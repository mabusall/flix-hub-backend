namespace FlixHub.Core.Api.Tasks;

internal class SyncMovieGenres(IFlixHubDbUnitOfWork uow,
                               TmdbService tmdbService,
                               IManagedCancellationToken applicationLifetime)
    : IHangfireJob
{
    public async Task ExecuteAsync()
    {
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
                    .FirstOrDefaultAsync(g => g.TmdbReferenceId == genre.Id, applicationLifetime.Token);

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
        await uow.SaveChangesAsync(applicationLifetime.Token);
    }
}