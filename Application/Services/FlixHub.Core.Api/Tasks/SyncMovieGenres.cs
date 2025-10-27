namespace FlixHub.Core.Api.Tasks;

internal class SyncMovieGenres(IFlixHubDbUnitOfWork uow,
                               TmdbService tmdbService,
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

        var currentDate = DateTime.UtcNow.Date;
        var movieGenres = await tmdbService.Movies.GetGenresAsync();
        var tvGenres = await tmdbService.Tv.GetGenresAsync();
        var genres = movieGenres.Genres
            .Concat(tvGenres.Genres)
            .GroupBy(g => g.Id)
            .Select(g => g.First())
            .OrderBy(g => g.Name)
            .ToList();

        foreach (var genre in genres)
        {
            var existingGenre = await uow
                    .GenresRepository
                    .AsQueryable(false)
                    .FirstOrDefaultAsync(g => g.TmdbReferenceId == genre.Id, appToken.Token);

            if (existingGenre is null)
            {
                var newGenre = new Genre
                {
                    TmdbReferenceId = genre.Id,
                    Name = genre.Name,
                };

                uow.GenresRepository.Insert(newGenre);
            }
            else if (existingGenre.Name != genre.Name)
            {
                existingGenre.TmdbReferenceId = genre.Id;
                existingGenre.Name = genre.Name;

                uow.GenresRepository.Update(existingGenre);
            }
        }

        await uow.SaveChangesAsync(appToken.Token);

        // Always release the semaphore
        _syncSemaphore.Release();
    }
}