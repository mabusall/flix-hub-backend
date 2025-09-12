namespace FlixHub.Core.Api.Services;

internal class TraktService(IApiClient apiClient,
                            IAppSettingsKeyManagement appSettings,
                            IManagedCancellationToken managedCancellationToken)
{
    public IntegrationApi TraktConf { get; set; } = appSettings.IntegrationApisOptions.Apis["TRAKT"];


    public async Task<TraktMovieResponse?> GetMovieSummaryAsync(string traktId)
    {
        return await apiClient.GetAsync<TraktMovieResponse>(TraktConf.BaseUrl,
                                                            $"movies/{traktId}",
                                                            null,
                                                            null,
                                                            managedCancellationToken.Token);
    }
}
