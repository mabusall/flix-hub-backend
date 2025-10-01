namespace FlixHub.Core.Api.Tasks;

internal class SyncContents(IFlixHubDbUnitOfWork uow,
                            TmdbService tmdbService,
                            OmdbService omdbService,
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
        var todayRequests = await GetTodayRequests();
        var totalRequestsToday = todayRequests.Sum(sum => sum.RequestCount);

        if (totalRequestsToday >= MaxDailyRequests)
            return;

        var remainingRequests = MaxDailyRequests - totalRequestsToday;
        var movieRequestsUsed = todayRequests
                .Where(daily => daily.ContentType == ContentType.Movie)
                .Sum(sum => sum.RequestCount);
        var tvRequestsUsed = todayRequests
                .Where(daily => daily.ContentType == ContentType.Series)
                .Sum(sum => sum.RequestCount);

        // ✅ PROFESSIONAL BALANCED 50/50 ALLOCATION
        var shouldSyncMovies = movieRequestsUsed < MovieQuota &&
                              (tvRequestsUsed >= TvQuota || movieRequestsUsed <= tvRequestsUsed);

        if (shouldSyncMovies)
        {
            await FetchNextMoviesBatch(Math.Min(remainingRequests, MovieQuota - movieRequestsUsed));
        }
        else if (!shouldSyncMovies && tvRequestsUsed < TvQuota)
        {
            //await FetchNextSeriesBatch(Math.Min(remainingRequests, TvQuota - tvRequestsUsed));
        }
    }

    /// <summary>
    /// ✅ PROFESSIONAL Movie Batch Processing with ContentSyncLog Management
    /// </summary>
    private async Task FetchNextMoviesBatch(int maxRequests)
    {
        var requestsUsed = 0;

        // ✅ FIND NEXT INCOMPLETE ContentSyncLog
        var syncLog = await uow.ContentSyncLogsRepository
            .AsQueryable(true)
            .Where(sync => sync.Type == ContentType.Movie && (sync.LastCompletedPage < sync.TotalPages || sync.TotalPages == null))
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .FirstOrDefaultAsync(appToken.Token);

        if (syncLog is null)
            return;

        var nextPage = syncLog.LastCompletedPage + 1;
        var lastDayOfMonth = DateTime.DaysInMonth(syncLog.Year, syncLog.Month);

        while (requestsUsed < maxRequests)
        {
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
                discoverResponse.TotalResults > 0 &&
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

            // check if the movie exists in the database, before sync it again
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

                // ratings
                var ratings = await omdbService
                    .GetMovieDetailsAsync(externalIds.ImdbId!);

                // create content
                // create method to build the content that accept
                // item.GenreIds, details, credits, images, videos, externalIds, ratings

                // increment page
                nextPage++;
                syncLog.LastCompletedPage = nextPage;
                syncLog.Notes = $"🎬 Processing movies {syncLog.Year}-{syncLog.Month:D2}, page {nextPage}";
                LogSyncNote(syncLog);

                // commit database changes
                // await uow.SaveChangesAsync(appToken.Token);

                #endregion
            }
        }
    }

    /// <summary>
    /// ✅ PROFESSIONAL TV Series Batch Processing with ContentSyncLog Management
    /// </summary>
    //private async Task FetchNextSeriesBatch(int maxRequests)
    //{
    //    var requestsUsed = 0;

    //    try
    //    {
    //        // ✅ FIND NEXT INCOMPLETE TV ContentSyncLog
    //        var syncLog = await uow.ContentSyncLogsRepository
    //            .AsQueryable(false)
    //            .Where(x => x.Type == ContentType.Series && !x.IsCompleted)
    //            .OrderBy(x => x.Year)
    //            .ThenBy(x => x.Month)
    //            .FirstOrDefaultAsync(appToken.Token);

    //        if (syncLog == null)
    //        {
    //            await LogSyncNote("✅ No incomplete TV sync logs - all series synchronized", ContentType.Series);
    //            return;
    //        }

    //        var nextPage = syncLog.LastCompletedPage + 1;
    //        var lastDayOfMonth = DateTime.DaysInMonth(syncLog.Year, syncLog.Month);

    //        var query = new Dictionary<string, string>
    //        {
    //            ["first_air_date.gte"] = $"{syncLog.Year}-{syncLog.Month:D2}-01",
    //            ["first_air_date.lte"] = $"{syncLog.Year}-{syncLog.Month:D2}-{lastDayOfMonth:D2}",
    //            ["sort_by"] = "first_air_date.asc",
    //            ["include_adult"] = "false",
    //            ["include_video"] = "false"
    //        };

    //        await LogSyncNote($"📺 Processing TV series {syncLog.Year}-{syncLog.Month:D2}, page {nextPage}", ContentType.Series);

    //        var discoverResponse = await tmdbService.Tv.GetDiscoverAsync("en-US", query, nextPage);
    //        requestsUsed++;

    //        if (syncLog.TotalPages == null)
    //        {
    //            syncLog.TotalPages = discoverResponse.TotalPages;
    //            uow.ContentSyncLogsRepository.Update(syncLog);
    //        }

    //        // ✅ PROCESS EACH TV SHOW WITH SEASONS/EPISODES NAVIGATION
    //        foreach (var tvItem in discoverResponse.Results)
    //        {
    //            if (requestsUsed >= maxRequests) break;

    //            try
    //            {
    //                await ProcessTvShowWithNavigationProperties(tvItem.Id);
    //                requestsUsed += 5; // TV shows need more requests (seasons/episodes)
    //            }
    //            catch (Exception ex)
    //            {
    //                await LogSyncNote($"❌ Error processing TV {tvItem.Id}: {ex.Message}", ContentType.Series);
    //            }
    //        }

    //        // ✅ PROFESSIONAL TV PROGRESS TRACKING
    //        syncLog.LastCompletedPage = nextPage;
    //        if (nextPage >= syncLog.TotalPages)
    //        {
    //            syncLog.IsCompleted = true;
    //            syncLog.Notes = $"✅ Completed: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC, Pages: {syncLog.TotalPages}";
    //        }

    //        uow.ContentSyncLogsRepository.Update(syncLog);
    //        //await uow.SaveChangesAsync(appToken.Token);
    //    }
    //    catch (Exception ex)
    //    {
    //        await LogSyncNote($"💥 CRITICAL TvBatch error: {ex.Message}", ContentType.Series);
    //    }
    //}

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