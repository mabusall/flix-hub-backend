namespace FlixHub.Core.Api.Tasks;

internal class SyncContentLog(IFlixHubDbUnitOfWork uow,
                              IManagedCancellationToken appToken)
    : IHangfireJob
{
    // ✅ Static semaphore to ensure only one execution at a time across all instances
    private static readonly SemaphoreSlim _syncSemaphore = new(1, 1);

    [DisableConcurrentExecution(timeoutInSeconds: 5 * 60)] // 10 minutes max
    public async Task ExecuteAsync()
    {
        // Try to acquire the semaphore, but don't wait if another instance is running
        if (!await _syncSemaphore.WaitAsync(0, appToken.Token))
        {
            // Another instance is already running, exit gracefully
            return;
        }

        var currentYear = DateTime.UtcNow.Year;

        // Movies: 1900 → current year
        for (int year = 1900; year <= currentYear; year++)
        {
            for (int month = 1; month <= 12; month++)
            {
                await InsertIfNotExists(ContentType.Movie, year, month);
            }
        }

        // Series: 1950 → current year (TV existed later)
        for (int year = 1950; year <= currentYear; year++)
        {
            for (int month = 1; month <= 12; month++)
            {
                await InsertIfNotExists(ContentType.Series, year, month);
            }
        }

        await uow.SaveChangesAsync(appToken.Token);

        // Always release the semaphore
        _syncSemaphore.Release();
    }

    private async Task InsertIfNotExists(ContentType type, int year, int month)
    {
        var exists = await uow.ContentSyncLogsRepository
            .AsQueryable(false)
            .AnyAsync(x => x.Type == type && x.Year == year && x.Month == month);

        if (!exists)
        {
            var log = new ContentSyncLog
            {
                Type = type,
                Year = year,
                Month = month,
                IsCompleted = false,
                LastCompletedPage = 0,
                TotalPages = null,
                Notes = null
            };

            uow.ContentSyncLogsRepository.Insert(log);
        }
    }
}