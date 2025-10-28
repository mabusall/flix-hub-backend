namespace FlixHub.Core.Api.Tasks;

internal class SyncContents(IFlixHubDbUnitOfWork uow,
                            IManagedCancellationToken appToken,
                            IAppSettingsKeyManagement appSettings,
                            TmdbService tmdbService,
                            OmdbService omdbService)
    : IHangfireJob
{
    private readonly int _maxDailyRequests = appSettings.DailySyncRequestsOptions.Limit;
    private readonly int _movieQuota = appSettings.DailySyncRequestsOptions.MovieQuota;
    private readonly int _tvQuota = appSettings.DailySyncRequestsOptions.SeriesQuota;

    // ✅ Static semaphore to ensure only one execution at a time across all instances
    private static readonly SemaphoreSlim _syncSemaphore = new(1, 1);

    //[DisableConcurrentExecution(timeoutInSeconds: 5 * 60)] // 10 minutes max
    /// <summary>
    /// Professional SyncContents ExecuteAsync - Balanced Movie/TV Sync with ContentSyncLog Management
    /// Prevents concurrent execution using SemaphoreSlim
    /// </summary>
    public async Task ExecuteAsync()
    {
        // Try to acquire the semaphore, but don't wait if another instance is running
        if (!await _syncSemaphore.WaitAsync(0, appToken.Token))
        {
            // Another instance is already running, exit gracefully
            return;
        }

        try
        {
            var todayRequests = await GetTodayRequests();
            var totalRequestsToday = todayRequests.Sum(sum => sum.RequestCount);

            if (totalRequestsToday >= _maxDailyRequests)
                return;

            var movieRequestsUsed = todayRequests
                    .Where(daily => daily.ContentType == ContentType.Movie)
                    .Sum(sum => sum.RequestCount);
            var tvRequestsUsed = todayRequests
                    .Where(daily => daily.ContentType == ContentType.Series)
                    .Sum(sum => sum.RequestCount);

            // ✅ PROFESSIONAL BALANCED 50/50 ALLOCATION
            if (movieRequestsUsed < _movieQuota)
            {
                await FetchNextMoviesBatch(_movieQuota);
            }
            else if (tvRequestsUsed < _tvQuota)
            {
                await FetchNextSeriesBatch(_tvQuota);
            }
        }
        finally
        {
            // Always release the semaphore
            _syncSemaphore.Release();
        }
    }

    /// <summary>
    /// Fetches the next batch of movies from the TMDb API and updates the content synchronization log.
    /// </summary>
    /// <remarks>This method retrieves movie data from the TMDb API based on the next incomplete entry in the
    /// content synchronization log. It processes each movie by fetching additional details such as genres, credits,
    /// images, and videos, and then stores the information in the database. The method continues fetching data until
    /// the maximum number of requests is reached or all pages for the current month are processed. If the
    /// synchronization log indicates completion, the log is updated accordingly.</remarks>
    /// <param name="maxRequests">The maximum number of API requests allowed during this operation. Must be a positive integer.</param>
    /// <returns></returns>
    private async Task FetchNextMoviesBatch(int maxRequests)
    {
        var requestsUsed = 0;

        while (requestsUsed < maxRequests)
        {
            // ✅ FIND NEXT INCOMPLETE ContentSyncLog
            var syncLog = await uow
                .ContentSyncLogsRepository
                .AsQueryable(true)
                .Where(sync => sync.Type == ContentType.Movie && !sync.IsCompleted && (sync.LastCompletedPage < sync.TotalPages || sync.TotalPages == null))
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .FirstOrDefaultAsync(appToken.Token);

            if (syncLog is null)
                return;

            while (true)
            {
                var nextPage = syncLog.LastCompletedPage + 1;
                var lastDayOfMonth = DateTime.DaysInMonth(syncLog.Year, syncLog.Month);

                // ✅ PROFESSIONAL TMDb QUERY CONSTRUCTION
                var query = new Dictionary<string, string>
                {
                    ["region"] = "US",
                    ["with_release_type"] = "2|3|4|5|6",
                    ["primary_release_date.gte"] = $"{syncLog.Year}-{syncLog.Month:D2}-01",
                    ["primary_release_date.lte"] = $"{syncLog.Year}-{syncLog.Month:D2}-{lastDayOfMonth:D2}",
                    ["sort_by"] = "primary_release_date.asc",
                    ["include_adult"] = "true",
                    ["include_video"] = "false"
                };

                var discoverResponse = await tmdbService
                    .Movies
                    .GetDiscoverAsync("en-US", query, nextPage);
                requestsUsed = await IncrementDailyApiUsage(ContentType.Movie);

                // ✅ PROFESSIONAL TOTAL PAGES TRACKING
                if (discoverResponse.TotalPages > 0 &&
                    syncLog.TotalPages is null)
                {
                    syncLog.TotalPages ??= discoverResponse.TotalPages;
                    syncLog.Notes = $"📊 Total pages: {discoverResponse.TotalPages} for {syncLog.Year}-{syncLog.Month:D2}";
                    LogSyncNote(syncLog);

                    // used for first time setup when TotalPages is null
                    await uow.SaveChangesAsync(appToken.Token);
                }

                // always check the request used before asking for the next request
                if (!ValidForNextRequest(requestsUsed, maxRequests)) break;

                #region [ loop through each movie and fetch details ]

                foreach (var item in discoverResponse.Results)
                {
                    // skip and check next movie
                    if (await ContentMediaExists(item.Id)) continue;

                    #region [ fetch genres, casts, crews, ratings, images, videos ]

                    // genres
                    var genres = await uow
                        .GenresRepository
                        .AsQueryable(false)
                        .Where(g => item.GenreIds.Contains(g.TmdbReferenceId))
                        .Select(s => new ContentGenre
                        {
                            GenreId = s.Id,
                        })
                        .ToListAsync();

                    // always check the request used before asking for the next request
                    if (!ValidForNextRequest(requestsUsed, maxRequests)) break;

                    // get movie details
                    var details = await tmdbService
                        .Movies
                        .GetDetailsAsync(item.Id);
                    requestsUsed = await IncrementDailyApiUsage(ContentType.Movie);

                    // always check the request used before asking for the next request
                    if (!ValidForNextRequest(requestsUsed, maxRequests)) break;

                    // casts and crews
                    var credits = await tmdbService
                        .Movies
                        .GetCreditsAsync(item.Id);
                    requestsUsed = await IncrementDailyApiUsage(ContentType.Movie);

                    // always check the request used before asking for the next request
                    if (!ValidForNextRequest(requestsUsed, maxRequests)) break;

                    // images
                    var images = await tmdbService
                        .Movies
                        .GetImagesAsync(item.Id);
                    requestsUsed = await IncrementDailyApiUsage(ContentType.Movie);

                    // always check the request used before asking for the next request
                    if (!ValidForNextRequest(requestsUsed, maxRequests)) break;

                    // videos
                    var videos = await tmdbService
                        .Movies
                        .GetVideosAsync(item.Id);
                    requestsUsed = await IncrementDailyApiUsage(ContentType.Movie);

                    // external ids
                    var externalIds = await tmdbService
                        .Movies
                        .GetExternalIdsAsync(item.Id);
                    requestsUsed = await IncrementDailyApiUsage(ContentType.Movie);

                    // ratings, awards
                    var omdbResponse = externalIds.ImdbId is null ? default : await omdbService
                        .GetImdbDetailsAsync(externalIds.ImdbId!);

                    // create content
                    var content = await BuildMovieMediaContent(item.GenreIds,
                                                               details,
                                                               credits,
                                                               images,
                                                               videos,
                                                               externalIds,
                                                               omdbResponse);
                    uow.ContentsRepository.Insert(content);

                    #endregion
                }

                #endregion

                #region [ increment page, check and commit changes ]

                // check if we reached the last page
                if (nextPage == discoverResponse.TotalPages)
                {
                    syncLog.LastCompletedPage = discoverResponse.TotalPages;
                    syncLog.IsCompleted = true;
                    syncLog.Notes = $"✅ Completed syncing movies for {syncLog.Year}-{syncLog.Month:D2}";
                    LogSyncNote(syncLog);

                    // commit database changes
                    await uow.SaveChangesAsync(appToken.Token);

                    // exit the while loop to find the next sync log
                    break;
                }
                else
                {
                    // increment page
                    syncLog.LastCompletedPage = nextPage;
                    nextPage++;
                }

                // commit database changes
                await uow.SaveChangesAsync(appToken.Token);

                #endregion
            }
        }
    }

    /// <summary>
    /// Fetches the next batch of movie series data from the TMDb API and updates the content synchronization log.
    /// </summary>
    /// <remarks>This method retrieves movie data for incomplete synchronization logs, processes the data, and
    /// updates the database. It continues fetching data until the specified maximum number of requests is reached or
    /// the synchronization is complete. The method ensures that API usage is tracked and respects the request
    /// limit.</remarks>
    /// <param name="maxRequests">The maximum number of API requests allowed during this operation. Must be a positive integer.</param>
    /// <returns></returns>
    private async Task FetchNextSeriesBatch(int maxRequests)
    {
        var requestsUsed = 0;

        while (requestsUsed < maxRequests)
        {
            // ✅ FIND NEXT INCOMPLETE ContentSyncLog
            var syncLog = await uow
                .ContentSyncLogsRepository
                .AsQueryable(true)
                .Where(sync => sync.Type == ContentType.Series && !sync.IsCompleted && (sync.LastCompletedPage < sync.TotalPages || sync.TotalPages == null))
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .FirstOrDefaultAsync(appToken.Token);

            // no more sync logs to process
            if (syncLog is null)
                break;

            while (true)
            {
                var nextPage = syncLog.LastCompletedPage + 1;
                var lastDayOfMonth = DateTime.DaysInMonth(syncLog.Year, syncLog.Month);

                // ✅ PROFESSIONAL TMDb QUERY CONSTRUCTION
                var query = new Dictionary<string, string>
                {
                    ["first_air_date.gte"] = $"{syncLog.Year}-{syncLog.Month:D2}-01",
                    ["first_air_date.lte"] = $"{syncLog.Year}-{syncLog.Month:D2}-{lastDayOfMonth:D2}",
                    ["sort_by"] = "first_air_date.asc",
                    ["include_adult"] = "true",
                    ["include_video"] = "false"
                };

                var discoverResponse = await tmdbService
                    .Tv
                    .GetDiscoverAsync("en-US", query, nextPage);
                requestsUsed = await IncrementDailyApiUsage(ContentType.Series);

                // ✅ PROFESSIONAL TOTAL PAGES TRACKING
                if (discoverResponse.TotalPages > 0 &&
                    syncLog.TotalPages is null)
                {
                    syncLog.TotalPages ??= discoverResponse.TotalPages;
                    syncLog.Notes = $"📊 Total pages: {discoverResponse.TotalPages} for {syncLog.Year}-{syncLog.Month:D2}";
                    LogSyncNote(syncLog);

                    // used for first time setup when TotalPages is null
                    await uow.SaveChangesAsync(appToken.Token);
                }

                // always check the request used before asking for the next request
                if (!ValidForNextRequest(requestsUsed, maxRequests)) break;

                #region [ loop through each series and fetch details ]

                foreach (var item in discoverResponse.Results)
                {
                    // skip and check next movie
                    if (await ContentMediaExists(item.Id)) continue;

                    #region [ fetch genres, casts, crews, ratings, images, videos ]

                    // genres
                    var genres = await uow
                        .GenresRepository
                        .AsQueryable(false)
                        .Where(g => item.GenreIds.Contains(g.TmdbReferenceId))
                        .Select(s => new ContentGenre
                        {
                            GenreId = s.Id,
                        })
                        .ToListAsync();

                    // always check the request used before asking for the next request
                    if (!ValidForNextRequest(requestsUsed, maxRequests)) break;

                    // get movie details
                    var details = await tmdbService
                        .Tv
                        .GetDetailsAsync(item.Id);
                    requestsUsed = await IncrementDailyApiUsage(ContentType.Series);

                    // always check the request used before asking for the next request
                    if (!ValidForNextRequest(requestsUsed, maxRequests)) break;

                    // casts and crews
                    var credits = await tmdbService
                        .Tv
                        .GetCreditsAsync(item.Id);
                    requestsUsed = await IncrementDailyApiUsage(ContentType.Series);

                    // always check the request used before asking for the next request
                    if (!ValidForNextRequest(requestsUsed, maxRequests)) break;

                    // images
                    var images = await tmdbService
                        .Tv
                        .GetImagesAsync(item.Id);
                    requestsUsed = await IncrementDailyApiUsage(ContentType.Series);

                    // always check the request used before asking for the next request
                    if (!ValidForNextRequest(requestsUsed, maxRequests)) break;

                    // videos
                    var videos = await tmdbService
                        .Tv
                        .GetVideosAsync(item.Id);
                    requestsUsed = await IncrementDailyApiUsage(ContentType.Series);

                    // external ids
                    var externalIds = await tmdbService
                        .Tv
                        .GetExternalIdsAsync(item.Id);
                    requestsUsed = await IncrementDailyApiUsage(ContentType.Series);

                    // ratings, awards
                    var omdbResponse = externalIds.ImdbId is null ? default : await omdbService
                        .GetImdbDetailsAsync(externalIds.ImdbId!);

                    // get seasons details
                    var (seasonDetails, totalRequests) = await BuildSeasons(details, maxRequests);

                    // always check the request used before asking for the next request
                    if (!ValidForNextRequest(totalRequests, maxRequests)) break;

                    // create content
                    var content = await BuildTvMediaContent(item.GenreIds,
                                                            details,
                                                            seasonDetails,
                                                            credits,
                                                            images,
                                                            videos,
                                                            externalIds,
                                                            omdbResponse);
                    uow.ContentsRepository.Insert(content);

                    #endregion
                }

                #endregion

                #region [ increment page, check and commit changes ]

                // check if we reached the last page
                if (nextPage == discoverResponse.TotalPages)
                {
                    syncLog.LastCompletedPage = discoverResponse.TotalPages;
                    syncLog.IsCompleted = true;
                    syncLog.Notes = $"✅ Completed syncing movies for {syncLog.Year}-{syncLog.Month:D2}";
                    LogSyncNote(syncLog);

                    // commit database changes
                    await uow.SaveChangesAsync(appToken.Token);

                    // exit the while loop to find the next sync log
                    break;
                }
                else
                {
                    // increment page
                    syncLog.LastCompletedPage = nextPage;
                    nextPage++;
                }

                // commit database changes
                await uow.SaveChangesAsync(appToken.Token);

                #endregion
            }
        }
    }

    /// <summary>
    /// Constructs a <see cref="Content"/> object representing a movie, using various data sources.
    /// </summary>
    /// <remarks>This method aggregates data from multiple sources to build a comprehensive representation of
    /// a movie. It includes handling of images, videos, genres, and other metadata. The method ensures that the
    /// constructed content object is populated with the most relevant and high-quality data available.</remarks>
    /// <param name="genreIds">A list of genre IDs associated with the movie.</param>
    /// <param name="movieDetails">The detailed information about the movie from TMDB.</param>
    /// <param name="credits">The cast and crew information from TMDB.</param>
    /// <param name="images">The image data from TMDB, including logos, backdrops, and posters.</param>
    /// <param name="videos">The video data from TMDB, including trailers and teasers.</param>
    /// <param name="externalIds">The external identifiers for the movie, such as IMDb ID.</param>
    /// <param name="omdbResponse">Optional detailed information about the movie from OMDB, including awards and ratings.</param>
    /// <returns>A <see cref="Content"/> object containing the aggregated movie data.</returns>
    private async Task<Content> BuildMovieMediaContent(IList<int> genreIds,
                                                       MovieDetailsResponse movieDetails,
                                                       TmdbCreditsResponse credits,
                                                       TmdbImagesResponse images,
                                                       TmdbVideosResponse videos,
                                                       TmdbExternalIdsResponse externalIds,
                                                       OmdbImdbDetailsResponse? omdbResponse)
    {
        if (movieDetails is null) return default!;

        var logoPath = images.Logos?
                            .Where(l => l.Iso6391 == "en")
                            .OrderByDescending(l => l.VoteAverage)
                            .FirstOrDefault()?.FilePath ??
                            images.Logos?
                            .OrderByDescending(l => l.VoteAverage)
                            .FirstOrDefault()?.FilePath;

        if (!string.IsNullOrWhiteSpace(logoPath))
            logoPath = $"{appSettings.IntegrationApisOptions.Apis["TMDB"].ResourcesUrl}/original{logoPath}";

        if (!string.IsNullOrWhiteSpace(movieDetails.PosterPath))
            movieDetails.PosterPath = $"{appSettings.IntegrationApisOptions.Apis["TMDB"].ResourcesUrl}/original{movieDetails.PosterPath}";

        var backdrops = images
                            .Backdrops
                            .OrderByDescending(o => o.VoteAverage)
                            .Select(s => new ContentImage
                            {
                                FilePath = $"{appSettings.IntegrationApisOptions.Apis["TMDB"].ResourcesUrl}/original{s.FilePath}",
                                Width = s.Width,
                                Height = s.Height,
                                Language = s.Iso6391,
                                Type = ImageType.Backdrop
                            })
                            .ToList();
        var posters = images
                        .Posters
                        .OrderByDescending(o => o.VoteAverage)
                        .Select(s => new ContentImage
                        {
                            FilePath = $"{appSettings.IntegrationApisOptions.Apis["TMDB"].ResourcesUrl}/original{s.FilePath}",
                            Width = s.Width,
                            Height = s.Height,
                            Language = s.Iso6391,
                            Type = ImageType.Poster
                        })
                        .ToList();
        var logos = images
                        .Logos?
                        .OrderByDescending(o => o.VoteAverage)
                        .Select(s => new ContentImage
                        {
                            FilePath = $"{appSettings.IntegrationApisOptions.Apis["TMDB"].ResourcesUrl}/original{s.FilePath}",
                            Width = s.Width,
                            Height = s.Height,
                            Language = s.Iso6391,
                            Type = ImageType.Logo
                        })
                        .ToList();

        List<ContentImage> allImages = [];
        allImages.AddRange(backdrops);
        allImages.AddRange(posters);
        if (logos is not null)
            allImages.AddRange(logos);

        var content = new Content
        {
            TmdbId = movieDetails.Id,
            ImdbId = externalIds?.ImdbId,
            Type = ContentType.Movie,
            IsAdult = movieDetails.Adult,
            Title = movieDetails.Title,
            OriginalTitle = movieDetails.OriginalTitle,
            Overview = movieDetails.Overview?.Length > 500 ? movieDetails.Overview[..500] : movieDetails.Overview,
            OriginalLanguage = movieDetails.OriginalLanguage,
            ReleaseDate = movieDetails.ReleaseDate,
            Runtime = movieDetails.Runtime,
            Popularity = (decimal)movieDetails.Popularity,
            VoteAverage = (decimal)movieDetails.VoteAverage,
            VoteCount = movieDetails.VoteCount,
            Budget = movieDetails.Budget,
            PosterPath = movieDetails.PosterPath,
            BackdropPath = movieDetails.BackdropPath,
            Genres = await uow
                        .GenresRepository
                        .AsQueryable(false)
                        .Where(g => genreIds.Contains(g.TmdbReferenceId))
                        .Select(s => new ContentGenre
                        {
                            GenreId = s.Id,
                        })
                        .ToListAsync(),
            Awards = omdbResponse?.Awards,
            Status = ParseContentStatus(movieDetails.Status),
            Ratings = omdbResponse is null ? null! : ParseContentRatings(omdbResponse?.Ratings),
            Country = omdbResponse?.Country,
            LogoPath = logoPath,
            Images = allImages,
            Casts = await MapCastMembers(credits.Cast),
            Crews = await MapCrewMembers(credits.Crew),
            Videos = [.. videos.Results
                        .Where(w => w.Site == "YouTube" && (w.Type == "Trailer" || w.Type == "Teaser"))
                        .OrderByDescending(o => o.Size)
                        .Select(s => new ContentVideo
                        {
                            Name = s.Name,
                            Key = s.Key,
                            Site = s.Site == "YouTube" ? VideoSite.YouTube : VideoSite.Vimeo,
                            Type = s.Type switch
                            {
                                "Trailer" => VideoType.Trailer,
                                "Teaser" => VideoType.Teaser,
                                "Clip" => VideoType.Clip,
                                "Featurette" => VideoType.Featurette,
                                _ => VideoType.Trailer
                            },
                            IsOfficial = s.Official,
                        })]
        };

        return content;
    }

    private async Task<Content> BuildTvMediaContent(IList<int> genreIds,
                                                    TvDetailsResponse tvDetails,
                                                    IList<ContentSeason> seasonDetails,
                                                    TmdbCreditsResponse credits,
                                                    TmdbImagesResponse images,
                                                    TmdbVideosResponse videos,
                                                    TmdbExternalIdsResponse externalIds,
                                                    OmdbImdbDetailsResponse? omdbResponse)
    {
        if (tvDetails is null) return default!;

        var logoPath = images.Logos?
                            .Where(l => l.Iso6391 == "en")
                            .OrderByDescending(l => l.VoteAverage)
                            .FirstOrDefault()?.FilePath ??
                            images.Logos?
                            .OrderByDescending(l => l.VoteAverage)
                            .FirstOrDefault()?.FilePath;

        if (!string.IsNullOrWhiteSpace(logoPath))
            logoPath = $"{appSettings.IntegrationApisOptions.Apis["TMDB"].ResourcesUrl}/original{logoPath}";

        if (!string.IsNullOrWhiteSpace(tvDetails.PosterPath))
            tvDetails.PosterPath = $"{appSettings.IntegrationApisOptions.Apis["TMDB"].ResourcesUrl}/original{tvDetails.PosterPath}";

        var backdrops = images
                            .Backdrops
                            .OrderByDescending(o => o.VoteAverage)
                            .Select(s => new ContentImage
                            {
                                FilePath = $"{appSettings.IntegrationApisOptions.Apis["TMDB"].ResourcesUrl}/original{s.FilePath}",
                                Width = s.Width,
                                Height = s.Height,
                                Language = s.Iso6391,
                                Type = ImageType.Backdrop
                            })
                            .ToList();
        var posters = images
                        .Posters
                        .OrderByDescending(o => o.VoteAverage)
                        .Select(s => new ContentImage
                        {
                            FilePath = $"{appSettings.IntegrationApisOptions.Apis["TMDB"].ResourcesUrl}/original{s.FilePath}",
                            Width = s.Width,
                            Height = s.Height,
                            Language = s.Iso6391,
                            Type = ImageType.Poster
                        })
                        .ToList();
        var logos = images
                        .Logos?
                        .OrderByDescending(o => o.VoteAverage)
                        .Select(s => new ContentImage
                        {
                            FilePath = $"{appSettings.IntegrationApisOptions.Apis["TMDB"].ResourcesUrl}/original{s.FilePath}",
                            Width = s.Width,
                            Height = s.Height,
                            Language = s.Iso6391,
                            Type = ImageType.Logo
                        })
                        .ToList();

        List<ContentImage> allImages = [];
        allImages.AddRange(backdrops);
        allImages.AddRange(posters);
        if (logos is not null)
            allImages.AddRange(logos);

        var content = new Content
        {
            TmdbId = tvDetails.Id,
            ImdbId = externalIds?.ImdbId,
            Type = ContentType.Series,
            IsAdult = tvDetails.Adult,
            Title = tvDetails.Name,
            OriginalTitle = tvDetails.OriginalName,
            Overview = tvDetails.Overview?.Length > 500 ? tvDetails.Overview[..500] : tvDetails.Overview,
            OriginalLanguage = tvDetails.OriginalLanguage,
            ReleaseDate = tvDetails.FirstAirDate,
            Popularity = (decimal)tvDetails.Popularity,
            VoteAverage = (decimal)tvDetails.VoteAverage,
            VoteCount = tvDetails.VoteCount,
            PosterPath = tvDetails.PosterPath,
            BackdropPath = tvDetails.BackdropPath,
            Genres = await uow
                        .GenresRepository
                        .AsQueryable(false)
                        .Where(g => genreIds.Contains(g.TmdbReferenceId))
                        .Select(s => new ContentGenre
                        {
                            GenreId = s.Id,
                        })
                        .ToListAsync(),
            Awards = omdbResponse?.Awards,
            Status = ParseContentStatus(tvDetails.Status),
            Ratings = omdbResponse is null ? null! : ParseContentRatings(omdbResponse?.Ratings),
            Country = omdbResponse?.Country,
            LogoPath = logoPath,
            Images = allImages,
            Casts = await MapCastMembers(credits.Cast),
            Crews = await MapCrewMembers(credits.Crew),
            Seasons = seasonDetails,
            Videos = [.. videos.Results
                        .Where(w => w.Site == "YouTube" && (w.Type == "Trailer" || w.Type == "Teaser"))
                        .OrderByDescending(o => o.Size)
                        .Select(s => new ContentVideo
                        {
                            Name = s.Name,
                            Key = s.Key,
                            Site = s.Site == "YouTube" ? VideoSite.YouTube : VideoSite.Vimeo,
                            Type = s.Type switch
                            {
                                "Trailer" => VideoType.Trailer,
                                "Teaser" => VideoType.Teaser,
                                "Clip" => VideoType.Clip,
                                "Featurette" => VideoType.Featurette,
                                _ => VideoType.Trailer
                            },
                            IsOfficial = s.Official,
                        })]
        };

        return content;
    }

    /// <summary>
    /// Asynchronously builds a list of content seasons based on the provided TV details.
    /// </summary>
    /// <remarks>This method retrieves season details for each season of the TV show specified in <paramref
    /// name="tvDetails"/>.</remarks>
    /// <param name="tvDetails">The details of the TV show, including the number of seasons.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see
    /// cref="ContentSeason"/> objects representing the seasons of the TV show.</returns>
    private async Task<(IList<ContentSeason>, int)> BuildSeasons(TvDetailsResponse tvDetails, int maxRequests)
    {
        var lstSeasonDetails = new List<ContentSeason>();
        var requestsUsed = 0;

        for (int season = 1; season <= tvDetails.NumberOfSeasons; season++)
        {
            try
            {
                var seasonDetails = await tmdbService
                    .Tv
                    .GetSeasonDetailsAsync(tvDetails.Id, season);
                requestsUsed = await IncrementDailyApiUsage(ContentType.Series);

                // always check the request used before asking for the next request
                if (!ValidForNextRequest(requestsUsed, maxRequests)) break;

                var episodes = new List<Episode>();

                if (seasonDetails.Episodes is not null)
                {
                    foreach (var e in seasonDetails.Episodes)
                    {
                        var crews = e.Crew is not null && e.Crew.Count > 0 ? await MapEpisodeCrewMembers(e.Crew) : [];

                        var episode = new Episode
                        {
                            EpisodeNumber = e.EpisodeNumber,
                            Title = e.Name ?? string.Empty,
                            Overview = e.Overview?.Length > 500 ? e.Overview[..500] : e.Overview,
                            AirDate = e.AirDate,
                            Runtime = e.Runtime,
                            StillPath = string.IsNullOrWhiteSpace(e.StillPath) ? null : $"{appSettings.IntegrationApisOptions.Apis["TMDB"].ResourcesUrl}/original{e.StillPath}",
                            VoteAverage = (decimal)e.VoteAverage,
                            VoteCount = e.VoteCount,
                            Crews = crews,
                        };

                        episodes.Add(episode);
                    }
                }

                lstSeasonDetails.Add(new ContentSeason
                {
                    SeasonNumber = seasonDetails.SeasonNumber,
                    Title = seasonDetails.Name,
                    Overview = seasonDetails.Overview?.Length > 500 ? seasonDetails.Overview[..500] : seasonDetails.Overview,
                    AirDate = seasonDetails.AirDate,
                    EpisodeCount = seasonDetails.Episodes?.Count,
                    PosterPath = string.IsNullOrWhiteSpace(seasonDetails.PosterPath) ? null : $"{appSettings.IntegrationApisOptions.Apis["TMDB"].ResourcesUrl}/original{seasonDetails.PosterPath}",
                    Episodes = episodes
                });
            }
            catch { }
        }

        return (lstSeasonDetails, requestsUsed);
    }

    private async Task<IList<EpisodeCrew>> MapEpisodeCrewMembers(IList<TmdbCrew> crew)
    {
        var episodeCrewMembers = new List<EpisodeCrew>();

        foreach (var crewMember in crew)
        {
            var episodeCrew = new EpisodeCrew
            {
                Department = crewMember.Department,
                Job = crewMember.Job,
                CreditId = crewMember.CreditId
            };

            // Try to find in the current context first (unsaved / tracked)
            var person = uow.Context()
                .Set<Person>().Local
                .FirstOrDefault(p => p.TmdbId == crewMember.Id);

            // If not found locally, check the database
            person ??= uow
                .PersonsRepository
                .AsQueryable(true)
                .FirstOrDefault(p => p.TmdbId == crewMember.Id);

            if (person is null)
            {
                // get person details from TMDb
                var personDetails = await tmdbService.People.GetDetailsAsync(crewMember.Id.ToString());
                person = MapPerson(personDetails);
            }

            episodeCrew.Person = person;
            episodeCrewMembers.Add(episodeCrew);
        }

        return episodeCrewMembers;
    }

    /// <summary>
    /// Maps a list of <see cref="TmdbCast"/> objects to a list of <see cref="ContentCast"/> objects.
    /// </summary>
    /// <param name="casts">The list of <see cref="TmdbCast"/> objects to be mapped. Cannot be null.</param>
    /// <returns>A list of <see cref="ContentCast"/> objects corresponding to the input <paramref name="casts"/>.</returns>
    private async Task<IList<ContentCast>> MapCastMembers(IList<TmdbCast> casts)
    {
        var contentCastMembers = new List<ContentCast>();

        foreach (var cast in casts)
        {
            var contentCast = new ContentCast
            {
                Character = cast.Character,
                Order = cast.Order,
                CreditId = cast.CreditId
            };

            // first, check if the person already exists in the database
            // Try to find in the current context first (unsaved / tracked)
            var person = uow
                .Context()
                .Set<Person>().Local
                .FirstOrDefault(p => p.TmdbId == cast.Id);

            // If not found locally, check the database
            person ??= uow
                .PersonsRepository
                .AsQueryable(true)
                .FirstOrDefault(p => p.TmdbId == cast.Id);

            if (person is null)
            {
                // get person details from TMDb
                var personDetails = await tmdbService.People.GetDetailsAsync(cast.Id.ToString());
                person = MapPerson(personDetails);
            }

            contentCast.Person = person;
            contentCastMembers.Add(contentCast);
        }

        return contentCastMembers;
    }

    /// <summary>
    /// Maps a list of TMDb crew members to a list of content cast members.
    /// </summary>
    /// <param name="crews">The list of TMDb crew members to be mapped. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of content cast members.</returns>
    private async Task<IList<ContentCrew>> MapCrewMembers(IList<TmdbCrew> crews)
    {
        var contentCrewMembers = new List<ContentCrew>();

        foreach (var crew in crews)
        {
            var contentCrew = new ContentCrew
            {
                Department = crew.Department,
                Job = crew.Job,
                CreditId = crew.CreditId
            };

            // first, check if the person already exists in the database
            // Try to find in the current context first (unsaved / tracked)
            var person = uow
                .Context()
                .Set<Person>()
                .Local
                .FirstOrDefault(p => p.TmdbId == crew.Id);

            // If not found locally, check the database
            person ??= uow
                    .PersonsRepository
                    .AsQueryable(true)
                    .FirstOrDefault(p => p.TmdbId == crew.Id);

            if (person is null)
            {
                // get person details from TMDb
                var personDetails = await tmdbService.People.GetDetailsAsync(crew.Id.ToString());
                person = MapPerson(personDetails);
            }

            contentCrew.Person = person;
            contentCrewMembers.Add(contentCrew);
        }

        return contentCrewMembers;
    }

    /// <summary>
    /// Maps a <see cref="PersonResponse"/> object to a <see cref="Person"/> object.
    /// </summary>
    /// <param name="personDetails">The <see cref="PersonResponse"/> containing the details to map.</param>
    /// <returns>A <see cref="Person"/> object populated with the mapped data from the <paramref name="personDetails"/>.</returns>
    private Person MapPerson(PersonResponse personDetails)
    {
        var person = new Person
        {
            TmdbId = personDetails.Id,
            DeathDate = personDetails.Deathday,
            KnownForDepartment = personDetails.KnownForDepartment,
            Gender = personDetails.Gender,
            Biography = personDetails.Biography,
            BirthPlace = personDetails.PlaceOfBirth,
            BirthDate = personDetails.Birthday,
            Name = personDetails.Name,
            PersonalPhoto = string.IsNullOrEmpty(personDetails.ProfilePath)
                    ? null
                    : $"{appSettings.IntegrationApisOptions.Apis["TMDB"].ResourcesUrl}/original{personDetails.ProfilePath}"
        };

        // insert new person to the database
        uow.PersonsRepository.Insert(person);

        return person;
    }

    /// <summary>
    /// ✅ PROFESSIONAL Accurate Daily API Requests - Uses DailyApiUsage Table
    /// </summary>
    private async Task<IList<DailyApiUsage>> GetTodayRequests()
    {
        var query = uow.DailyApiUsagesRepository
            .AsQueryable(false)
            .Where(x => x.Date == DateTime.UtcNow.Date);

        return await query.ToListAsync(appToken.Token);
    }

    /// <summary>
    /// ✅ PROFESSIONAL ContentSyncLog Notes Management
    /// </summary>
    private void LogSyncNote(ContentSyncLog recentLog,
                             string note = null!)
    {
        if (recentLog is not null)
        {
            if (string.IsNullOrEmpty(recentLog.Notes))
            {
                var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                recentLog.Notes = string.IsNullOrEmpty(recentLog.Notes)
                    ? $"[{timestamp}] {note}"
                    : $"{recentLog.Notes}\n[{timestamp}] {note}";
            }

            // Keep notes manageable
            if (recentLog.Notes.Length > 500)
            {
                var lines = recentLog.Notes.Split('\n');
                recentLog.Notes = string.Join('\n', lines.TakeLast(5));
            }

            uow.ContentSyncLogsRepository.Update(recentLog);
        }
    }

    /// <summary>
    /// ✅ PROFESSIONAL Status Parsing Helper
    /// </summary>
    private static ContentStatus? ParseContentStatus(string? status)
    {
        return status?.ToLower() switch
        {
            "released" => ContentStatus.Released,
            "ended" => ContentStatus.Ended,
            "returning series" => ContentStatus.Returning,
            _ => null
        };
    }

    private static IList<ContentRating> ParseContentRatings(IList<OmdbMovieDetailsResponseRating>? ratings)
    {
        if (ratings is null || ratings.Count == 0)
            return null!;

        return [.. ratings.Select(rating => new ContentRating
        {
            Value = rating.Value,
            Source = rating.Source switch
            {
                "Internet Movie Database" => RatingSource.InternetMovieDatabase,
                "Rotten Tomatoes" => RatingSource.RottenTomatoes,
                "Metacritic" => RatingSource.Metacritic,
                _ => RatingSource.Unknown
            }
        })];
    }

    /// <summary>
    /// <returns>A task representing the asynchronous database operation</returns>
    /// <remarks>
    /// The method performs the following operations:
    /// - Increments the provided requestCount reference parameter
    /// - Searches for existing daily usage record for today and the specified content type
    /// - If found, increments the existing record's count and updates the ref parameter
    /// - If not found, creates a new daily usage record starting with count of 1
    /// - Updates the database through the unit of work pattern but does not save changes
    /// </remarks>
    /// </summary>
    private async Task<int> IncrementDailyApiUsage(ContentType contentType)
    {
        int requestCount = 0;

        var today = DateTime.UtcNow.Date;

        // Try to find existing daily usage record for today and content type
        var existingUsage = await uow.DailyApiUsagesRepository
            .AsQueryable(true)
            .FirstOrDefaultAsync(x => x.Date == DateTime.UtcNow.Date && x.ContentType == contentType, appToken.Token);

        if (existingUsage is not null)
        {
            // Update existing record
            existingUsage.RequestCount++;
            requestCount = existingUsage.RequestCount;
            uow.DailyApiUsagesRepository.Update(existingUsage);
        }
        else
        {
            // Create new record for today
            var dailyUsage = new DailyApiUsage
            {
                Date = today,
                ContentType = contentType,
                RequestCount = 1 // Start with 1, not the total requestCount
            };
            requestCount = dailyUsage.RequestCount;
            uow.DailyApiUsagesRepository.Insert(dailyUsage);

            // commit changes to the database
            await uow.SaveChangesAsync(appToken.Token);
        }

        return requestCount;
    }

    /// <summary>
    /// Validates whether additional API requests can be made within the allocated quota limit.
    /// </summary>
    /// <param name="requestsUsed">The number of API requests already consumed in the current batch</param>
    /// <param name="maxRequests">The maximum number of requests allowed for the current batch</param>
    /// <returns>
    /// <c>true</c> if more requests can be made without exceeding the quota; 
    /// <c>false</c> if the request limit has been reached
    /// </returns>
    /// <remarks>
    /// This method serves as a rate limiting guard to prevent the sync process from exceeding 
    /// daily API quotas for TMDb and OMDb services. It ensures graceful quota management and 
    /// prevents potential service interruptions or additional charges from API providers.
    /// </remarks>
    private static bool ValidForNextRequest(int requestsUsed, int maxRequests)
        => requestsUsed < maxRequests;

    /// <summary>
    /// Checks if content media already exists in the database by TMDB ID.
    /// </summary>
    /// <param name="tmdbId">The TMDB identifier to search for in the database</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains 
    /// <c>true</c> if content with the specified TMDB ID exists; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method performs a database query to prevent duplicate content synchronization 
    /// by checking the Contents repository for existing records with the same TMDB reference ID.
    /// Uses read-only query tracking for optimal performance during sync operations.
    /// </remarks>
    private async Task<bool> ContentMediaExists(int tmdbId)
        => await uow.ContentsRepository
            .AsQueryable(false)
            .AnyAsync(c => c.TmdbId == tmdbId, appToken.Token);
}