namespace FlixHub.Core.Api.Tasks;

internal class SyncContents(IFlixHubDbUnitOfWork uow,
                            TmdbService tmdbService,
                            OmdbService omdbService,
                            TraktService traktService,
                            IManagedCancellationToken appToken)
    : IHangfireJob
{
    public async Task ExecuteAsync()
    {
    }
}