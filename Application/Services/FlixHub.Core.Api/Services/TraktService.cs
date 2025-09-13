namespace FlixHub.Core.Api.Services;

internal sealed class TraktService(IApiClient apiClient,
                                   IAppSettingsKeyManagement appSettings,
                                   IManagedCancellationToken managedCancellationToken)
{
    public IntegrationApi TraktConf { get; set; } = appSettings.IntegrationApisOptions.Apis["TRAKT"];

    private Dictionary<string, string> BuildHeaders() => new()
    {
        { "Authorization", $"Bearer {TraktConf.Token.Decrypt()}" }
    };

    public async Task<TraktMovieDetailsResponse> GetMovieDetailsAsync(string imdbId)
    {
        return await apiClient.GetAsync<TraktMovieDetailsResponse>(TraktConf.BaseUrl,
                                                                   $"movies/{imdbId}?extended=full",
                                                                   BuildHeaders(),
                                                                   null,
                                                                   managedCancellationToken.Token);
    }
}
