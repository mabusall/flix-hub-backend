namespace FlixHub.Core.Api.Services;

internal sealed class TmdbMovieService(IApiClient apiClient,
                                       IAppSettingsKeyManagement appSettings,
                                       IManagedCancellationToken managedCancellationToken)
{
    public IntegrationApi TmdbConf { get; set; } = appSettings.IntegrationApisOptions.Apis["TMDB"];

    private Dictionary<string, string> BuildHeaders() => new()
    {
        { "Authorization", $"Bearer {TmdbConf.Token.Decrypt()}" }
    };

    public async Task<TmdbDiscoverSearchTrendingResponse> GetDiscoverAsync(string? language = "en-US",
                                                                           Dictionary<string, string> query = default!,
                                                                           int? page = null)
    {
        if (!string.IsNullOrEmpty(language))
            query["language"] = language;
        if (page is not null)
            query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<TmdbDiscoverSearchTrendingResponse>(TmdbConf.BaseUrl,
                                                                            $"discover/movie",
                                                                            BuildHeaders(),
                                                                            query,
                                                                            managedCancellationToken.Token);
    }

    public async Task<TmdbDiscoverSearchTrendingResponse> GetSearchAsync(string? language = "en-US",
                                                                         Dictionary<string, string> query = default!,
                                                                         int? page = null)
    {
        if (!string.IsNullOrEmpty(language))
            query["language"] = language;
        if (page is not null)
            query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<TmdbDiscoverSearchTrendingResponse>(TmdbConf.BaseUrl,
                                                                            $"search/movie",
                                                                            BuildHeaders(),
                                                                            query,
                                                                            managedCancellationToken.Token);
    }

    public async Task<MovieDetailsResponse> GetDetailsAsync(int id)
    {
        return await apiClient.GetAsync<MovieDetailsResponse>(TmdbConf.BaseUrl,
                                                          $"movie/{id}",
                                                          BuildHeaders(),
                                                          null,
                                                          managedCancellationToken.Token);
    }

}