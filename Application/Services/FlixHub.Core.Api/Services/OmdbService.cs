namespace FlixHub.Core.Api.Services;

internal sealed class OmdbService(IApiClient apiClient,
                                  IAppSettingsKeyManagement appSettings,
                                  IManagedCancellationToken managedCancellationToken)
{
    public IntegrationApi OmdbConf { get; set; } = appSettings.IntegrationApisOptions.Apis["OMDB"];

    public async Task<OmdbMovieDetailsResponse> GetMovieDetailsAsync(string imdbId)
    {
        var query = new Dictionary<string, string>
        {
            { "i", imdbId },
            { "apikey", OmdbConf.Token.Decrypt() }
        };

        return await apiClient.GetAsync<OmdbMovieDetailsResponse>(OmdbConf.BaseUrl,
                                                                  null,
                                                                  null,
                                                                  query,
                                                                  managedCancellationToken.Token);
    }
}
