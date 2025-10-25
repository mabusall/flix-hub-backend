namespace FlixHub.Core.Api.Services;

internal sealed class TmdbPeopleService(IApiClient apiClient,
                                       IAppSettingsKeyManagement appSettings,
                                       IManagedCancellationToken managedCancellationToken)
{
    public IntegrationApi TmdbConf { get; set; } = appSettings.IntegrationApisOptions.Apis["TMDB"];

    private Dictionary<string, string> BuildHeaders() => new()
    {
        { "Authorization", $"Bearer {TmdbConf.Tokens.First().Decrypt()}" }
    };

    public async Task<PersonResponse> GetDetailsAsync(string id,
                                                      string? language = "en-US")
    {
        var query = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(language))
            query["language"] = language;

        return await apiClient.GetAsync<PersonResponse>(TmdbConf.BaseUrl,
                                                        $"person/{id}",
                                                        BuildHeaders(),
                                                        query,
                                                        managedCancellationToken.Token);
    }

}