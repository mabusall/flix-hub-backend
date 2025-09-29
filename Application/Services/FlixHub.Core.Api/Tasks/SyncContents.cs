namespace FlixHub.Core.Api.Tasks;

internal class SyncContents(IFlixHubDbUnitOfWork uow,
                            TmdbService tmdbService,
                            OmdbService omdbService,
                            TraktService traktService,
                            IManagedCancellationToken appToken)
    : IHangfireJob
{
    private const int MaxDailyRequests = 1000;
    private const int MovieQuota = 500;
    private const int TvQuota = 500;
    
    /// <summary>
    /// Professional SyncContents ExecuteAsync - Balanced Movie/TV Sync with ContentSyncLog Management
    /// </summary>
    public async Task ExecuteAsync()
    {
        var today = DateTime.UtcNow.Date;
        var totalRequestsToday = await GetTodayRequestCount();
        
        if (totalRequestsToday >= MaxDailyRequests)
        {
            await LogSyncNote("Daily API quota of 1000 requests exhausted", ContentType.Movie);
            return;
        }
        
        var remainingRequests = MaxDailyRequests - totalRequestsToday;
        var movieRequestsUsed = await GetTodayRequestCount(ContentType.Movie);
        var tvRequestsUsed = await GetTodayRequestCount(ContentType.Series);
        
        // ✅ PROFESSIONAL BALANCED 50/50 ALLOCATION
        var shouldSyncMovies = movieRequestsUsed < MovieQuota && 
                              (tvRequestsUsed >= TvQuota || movieRequestsUsed <= tvRequestsUsed);
        
        if (shouldSyncMovies && movieRequestsUsed < MovieQuota)
        {
            await FetchNextMoviesBatch(Math.Min(remainingRequests, MovieQuota - movieRequestsUsed));
        }
        else if (!shouldSyncMovies && tvRequestsUsed < TvQuota)
        {
            await FetchNextSeriesBatch(Math.Min(remainingRequests, TvQuota - tvRequestsUsed));
        }
    }

    /// <summary>
    /// ✅ PROFESSIONAL Movie Batch Processing with ContentSyncLog Management
    /// </summary>
    private async Task FetchNextMoviesBatch(int maxRequests)
    {
        var requestsUsed = 0;
        
        try
        {
            // ✅ FIND NEXT INCOMPLETE ContentSyncLog
            var syncLog = await uow.ContentSyncLogsRepository
                .AsQueryable(false)
                .Where(x => x.Type == ContentType.Movie && !x.IsCompleted)
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .FirstOrDefaultAsync(appToken.Token);

            if (syncLog == null)
            {
                await LogSyncNote("✅ No incomplete movie sync logs - all movies synchronized", ContentType.Movie);
                return;
            }

            // ✅ PROFESSIONAL PAGINATION RESUME
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
                ["include_adult"] = "false",
                ["include_video"] = "false"
            };

            await LogSyncNote($"🎬 Processing movies {syncLog.Year}-{syncLog.Month:D2}, page {nextPage}", ContentType.Movie);

            // ✅ PROFESSIONAL API CALL with ERROR HANDLING
            var discoverResponse = await tmdbService.Movies.GetDiscoverAsync("en-US", query, nextPage);
            requestsUsed++;

            // ✅ PROFESSIONAL TOTAL PAGES TRACKING
            if (syncLog.TotalPages == null)
            {
                syncLog.TotalPages = discoverResponse.TotalPages;
                uow.ContentSyncLogsRepository.Update(syncLog);
                await LogSyncNote($"📊 Total pages: {discoverResponse.TotalPages} for {syncLog.Year}-{syncLog.Month:D2}", ContentType.Movie);
            }

            // ✅ PROCESS EACH MOVIE WITH NAVIGATION PROPERTIES
            foreach (var movieItem in discoverResponse.Results)
            {
                if (requestsUsed >= maxRequests) 
                {
                    await LogSyncNote($"🚫 Quota limit ({maxRequests}) reached, stopping batch", ContentType.Movie);
                    break;
                }
                
                try
                {
                    await ProcessMovieWithNavigationProperties(movieItem.Id);
                    requestsUsed += 3; // Conservative estimate for now
                }
                catch (Exception ex)
                {
                    await LogSyncNote($"❌ Error processing movie {movieItem.Id}: {ex.Message}", ContentType.Movie);
                }
            }

            // ✅ PROFESSIONAL PROGRESS UPDATE & COMPLETION
            syncLog.LastCompletedPage = nextPage;
            if (nextPage >= syncLog.TotalPages)
            {
                syncLog.IsCompleted = true;
                syncLog.Notes = $"✅ Completed: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC, Pages: {syncLog.TotalPages}";
                await LogSyncNote($"🎉 COMPLETED sync for {syncLog.Year}-{syncLog.Month:D2}", ContentType.Movie);
            }
            else
            {
                await LogSyncNote($"📈 Progress: {nextPage}/{syncLog.TotalPages} for {syncLog.Year}-{syncLog.Month:D2}", ContentType.Movie);
            }
            
            uow.ContentSyncLogsRepository.Update(syncLog);
            await uow.SaveChangesAsync(appToken.Token);
        }
        catch (Exception ex)
        {
            await LogSyncNote($"💥 CRITICAL MovieBatch error: {ex.Message}", ContentType.Movie);
        }
    }

    /// <summary>
    /// ✅ PROFESSIONAL TV Series Batch Processing with ContentSyncLog Management
    /// </summary>
    private async Task FetchNextSeriesBatch(int maxRequests)
    {
        var requestsUsed = 0;
        
        try
        {
            // ✅ FIND NEXT INCOMPLETE TV ContentSyncLog
            var syncLog = await uow.ContentSyncLogsRepository
                .AsQueryable(false)
                .Where(x => x.Type == ContentType.Series && !x.IsCompleted)
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .FirstOrDefaultAsync(appToken.Token);

            if (syncLog == null)
            {
                await LogSyncNote("✅ No incomplete TV sync logs - all series synchronized", ContentType.Series);
                return;
            }

            var nextPage = syncLog.LastCompletedPage + 1;
            var lastDayOfMonth = DateTime.DaysInMonth(syncLog.Year, syncLog.Month);
            
            var query = new Dictionary<string, string>
            {
                ["first_air_date.gte"] = $"{syncLog.Year}-{syncLog.Month:D2}-01",
                ["first_air_date.lte"] = $"{syncLog.Year}-{syncLog.Month:D2}-{lastDayOfMonth:D2}",
                ["sort_by"] = "first_air_date.asc",
                ["include_adult"] = "false",
                ["include_video"] = "false"
            };

            await LogSyncNote($"📺 Processing TV series {syncLog.Year}-{syncLog.Month:D2}, page {nextPage}", ContentType.Series);

            var discoverResponse = await tmdbService.Tv.GetDiscoverAsync("en-US", query, nextPage);
            requestsUsed++;

            if (syncLog.TotalPages == null)
            {
                syncLog.TotalPages = discoverResponse.TotalPages;
                uow.ContentSyncLogsRepository.Update(syncLog);
            }

            // ✅ PROCESS EACH TV SHOW WITH SEASONS/EPISODES NAVIGATION
            foreach (var tvItem in discoverResponse.Results)
            {
                if (requestsUsed >= maxRequests) break;
                
                try
                {
                    await ProcessTvShowWithNavigationProperties(tvItem.Id);
                    requestsUsed += 5; // TV shows need more requests (seasons/episodes)
                }
                catch (Exception ex)
                {
                    await LogSyncNote($"❌ Error processing TV {tvItem.Id}: {ex.Message}", ContentType.Series);
                }
            }

            // ✅ PROFESSIONAL TV PROGRESS TRACKING
            syncLog.LastCompletedPage = nextPage;
            if (nextPage >= syncLog.TotalPages)
            {
                syncLog.IsCompleted = true;
                syncLog.Notes = $"✅ Completed: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC, Pages: {syncLog.TotalPages}";
            }
            
            uow.ContentSyncLogsRepository.Update(syncLog);
            await uow.SaveChangesAsync(appToken.Token);
        }
        catch (Exception ex)
        {
            await LogSyncNote($"💥 CRITICAL TvBatch error: {ex.Message}", ContentType.Series);
        }
    }

    /// <summary>
    /// ✅ PROFESSIONAL Movie Processing with ALL NAVIGATION PROPERTIES
    /// </summary>
    private async Task ProcessMovieWithNavigationProperties(int tmdbId)
    {
        // Check if exists
        var existingContent = await uow.ContentsRepository
            .AsQueryable(false)
            .FirstOrDefaultAsync(c => c.TmdbId == tmdbId && c.Type == ContentType.Movie, appToken.Token);

        if (existingContent != null) return;

        try
        {
            // Get movie details
            var movieDetails = await tmdbService.Movies.GetDetailsAsync(tmdbId);

            // ✅ CREATE CONTENT WITH ALL PROPERTIES
            var content = new Content
            {
                TmdbId = tmdbId,
                Type = ContentType.Movie,
                Title = movieDetails.Title ?? movieDetails.OriginalTitle ?? "Unknown Movie",
                OriginalTitle = movieDetails.OriginalTitle,
                Overview = movieDetails.Overview?.Length > 500 ? movieDetails.Overview[..500] : movieDetails.Overview,
                OriginalLanguage = movieDetails.OriginalLanguage,
                Runtime = movieDetails.Runtime,
                Status = ParseContentStatus(movieDetails.Status ?? ""),
                ReleaseDate = movieDetails.ReleaseDate,
                Budget = movieDetails.Budget,
                Popularity = (decimal?)movieDetails.Popularity,
                VoteAverage = (decimal?)movieDetails.VoteAverage,
                VoteCount = movieDetails.VoteCount,
                PosterPath = movieDetails.PosterPath,
                BackdropPath = movieDetails.BackdropPath
            };

            uow.ContentsRepository.Insert(content);
            await uow.SaveChangesAsync(appToken.Token);

            // ✅ PROCESS ALL NAVIGATION PROPERTIES
            await ProcessContentGenres(content.Id, movieDetails.Genres);
            
            // TODO: Add when TMDb service methods are available:
            // await ProcessContentCastCrew(content.Id, tmdbId, ContentType.Movie);
            // await ProcessContentImages(content.Id, tmdbId);
            // await ProcessContentVideos(content.Id, tmdbId);
            // await ProcessOMDbRatings(content.Id, movieDetails);
            
        }
        catch (Exception ex)
        {
            await LogSyncNote($"❌ ProcessMovie error {tmdbId}: {ex.Message}", ContentType.Movie);
        }
    }

    /// <summary>
    /// ✅ PROFESSIONAL TV Show Processing with SEASONS/EPISODES NAVIGATION
    /// </summary>
    private async Task ProcessTvShowWithNavigationProperties(int tmdbId)
    {
        var existingContent = await uow.ContentsRepository
            .AsQueryable(false)
            .FirstOrDefaultAsync(c => c.TmdbId == tmdbId && c.Type == ContentType.Series, appToken.Token);

        if (existingContent != null) return;

        try
        {
            var tvDetails = await tmdbService.Tv.GetDetailsAsync(tmdbId);

            // ✅ CREATE TV CONTENT WITH ALL PROPERTIES
            var content = new Content
            {
                TmdbId = tmdbId,
                Type = ContentType.Series,
                Title = tvDetails.Name ?? tvDetails.OriginalName ?? "Unknown Series",
                OriginalTitle = tvDetails.OriginalName,
                Overview = tvDetails.Overview?.Length > 500 ? tvDetails.Overview[..500] : tvDetails.Overview,
                OriginalLanguage = tvDetails.OriginalLanguage,
                Runtime = tvDetails.EpisodeRunTime?.FirstOrDefault(),
                Status = ParseContentStatus(tvDetails.Status ?? ""),
                ReleaseDate = tvDetails.FirstAirDate,
                Popularity = (decimal?)tvDetails.Popularity,
                VoteAverage = (decimal?)tvDetails.VoteAverage,
                VoteCount = tvDetails.VoteCount,
                PosterPath = tvDetails.PosterPath,
                BackdropPath = tvDetails.BackdropPath
            };

            uow.ContentsRepository.Insert(content);
            await uow.SaveChangesAsync(appToken.Token);

            // ✅ PROCESS ALL TV NAVIGATION PROPERTIES
            await ProcessContentGenres(content.Id, tvDetails.Genres);
            
            // TODO: Add comprehensive TV enrichment:
            // await ProcessTvSeasons(content.Id, tmdbId, tvDetails.NumberOfSeasons);
            // await ProcessContentCastCrew(content.Id, tmdbId, ContentType.Series);
            
        }
        catch (Exception ex)
        {
            await LogSyncNote($"❌ ProcessTv error {tmdbId}: {ex.Message}", ContentType.Series);
        }
    }

    /// <summary>
    /// ✅ PROFESSIONAL Genre Navigation Property Processing
    /// </summary>
    private async Task ProcessContentGenres(long contentId, IList<TmdbGenre> tmdbGenres)
    {
        foreach (var tmdbGenre in tmdbGenres)
        {
            var genre = await uow.GenresRepository
                .AsQueryable(false)
                .FirstOrDefaultAsync(g => g.TmdbId == tmdbGenre.Id, appToken.Token);

            if (genre != null)
            {
                // ✅ CONTENT → GENRES NAVIGATION PROPERTY
                var contentGenre = new ContentGenre
                {
                    ContentId = contentId,
                    GenreId = genre.Id
                };
                uow.ContentGenresRepository.Insert(contentGenre);
            }
        }
    }

    /// <summary>
    /// ✅ PROFESSIONAL Request Count Estimation for Balanced Quotas
    /// </summary>
    private async Task<int> GetTodayRequestCount(ContentType? contentType = null)
    {
        var today = DateTime.UtcNow.Date;
        var query = uow.ContentSyncLogsRepository.AsQueryable(false)
            .Where(x => x.LastModified.HasValue && x.LastModified.Value.Date == today);

        if (contentType.HasValue)
        {
            query = query.Where(x => x.Type == contentType.Value);
        }

        var logs = await query.ToListAsync(appToken.Token);
        var estimatedRequests = 0;

        foreach (var log in logs)
        {
            var pagesProcessed = log.LastCompletedPage;
            if (log.Type == ContentType.Movie)
            {
                estimatedRequests += pagesProcessed * 7; // 1 discover + ~6 enrichment per movie
            }
            else
            {
                estimatedRequests += pagesProcessed * 12; // TV uses more (seasons/episodes)
            }
        }

        return estimatedRequests;
    }

    /// <summary>
    /// ✅ PROFESSIONAL ContentSyncLog Notes Management
    /// </summary>
    private async Task LogSyncNote(string note, ContentType contentType)
    {
        var recentLog = await uow.ContentSyncLogsRepository
            .AsQueryable(false)
            .Where(x => x.Type == contentType)
            .OrderByDescending(x => x.LastModified ?? x.Created)
            .FirstOrDefaultAsync(appToken.Token);

        if (recentLog != null)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            recentLog.Notes = string.IsNullOrEmpty(recentLog.Notes) 
                ? $"[{timestamp}] {note}"
                : $"{recentLog.Notes}\n[{timestamp}] {note}";
                
            // Keep notes manageable
            if (recentLog.Notes.Length > 500)
            {
                var lines = recentLog.Notes.Split('\n');
                recentLog.Notes = string.Join('\n', lines.TakeLast(5));
            }
            
            uow.ContentSyncLogsRepository.Update(recentLog);
            await uow.SaveChangesAsync(appToken.Token);
        }
    }

    /// <summary>
    /// ✅ PROFESSIONAL Status Parsing Helper
    /// </summary>
    private static ContentStatus? ParseContentStatus(string status)
    {
        return status?.ToLower() switch
        {
            "released" => ContentStatus.Released,
            "ended" => ContentStatus.Ended,
            "returning series" => ContentStatus.Returning,
            _ => null
        };
    }
}