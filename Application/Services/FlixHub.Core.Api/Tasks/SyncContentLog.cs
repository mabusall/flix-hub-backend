namespace FlixHub.Core.Api.Tasks;

internal class SyncContentLog(IFlixHubDbUnitOfWork uow,
                              IManagedCancellationToken applicationLifetime)
    : IHangfireJob
{
    public async Task ExecuteAsync()
    {
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
        for (int year = 1941; year <= currentYear; year++)
        {
            for (int month = 1; month <= 12; month++)
            {
                await InsertIfNotExists(ContentType.Series, year, month);
            }
        }

        await uow.SaveChangesAsync(applicationLifetime.Token);
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