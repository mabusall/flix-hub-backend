namespace FlixHub.Core.Api.Tasks;

internal class SyncMovieGenres(IFlixHubDbUnitOfWork uow,
                               TmdbService tmdbService,
                               IManagedCancellationToken applicationLifetime)
    : IHangfireJob
{
    public async Task ExecuteAsync()
    {
        var currentDate = DateTime.UtcNow.Date;
        var response = await tmdbService.GetGenresAsync("en");

        foreach (var genre in response!.Genres)
        {
            var existingGenre = await uow
                    .GenresRepository
                    .AsQueryable(false)
                    .FirstOrDefaultAsync(g => g.TmdbId == genre.Id, applicationLifetime.Token);

            if (existingGenre is null)
            {
                var newGenre = new Genre
                {
                    TmdbId = genre.Id,
                    Name = genre.Name,
                };

                uow.GenresRepository.Insert(newGenre);
            }
            else if (existingGenre.Name != genre.Name)
            {
                existingGenre.TmdbId = genre.Id;
                existingGenre.Name = genre.Name;

                uow.GenresRepository.Update(existingGenre);
            }
        }
        await uow.SaveChangesAsync(applicationLifetime.Token);
    }
}