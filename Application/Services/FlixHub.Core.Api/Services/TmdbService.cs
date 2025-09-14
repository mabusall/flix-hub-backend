namespace FlixHub.Core.Api.Services;

internal sealed class TmdbService(IApiClient apiClient,
                                  IAppSettingsKeyManagement appSettings,
                                  IManagedCancellationToken managedCancellationToken)
{
    public TmdbMovieService Movies { get; } = new(apiClient, appSettings, managedCancellationToken);
    public TmdbTvService Tv { get; } = new(apiClient, appSettings, managedCancellationToken);
    public TmdbPeopleService People { get; } = new(apiClient, appSettings, managedCancellationToken);
}
