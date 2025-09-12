namespace FlixHub.Core.Api.Services;

internal class TmdbService(IApiClient apiClient,
                          IAppSettingsKeyManagement appSettings,
                          IManagedCancellationToken managedCancellationToken)
{
    public IntegrationApi TmdbConf { get; set; } = appSettings.IntegrationApisOptions.Apis["TMDB"];

    private Dictionary<string, string> BuildHeaders() => new()
    {
        { "Authorization", $"Bearer {TmdbConf.Token.Decrypt()}" }
    };

    public async Task<TmdbGenreResponse?> GetGenresAsync(string language = "en")
    {
        return await apiClient.GetAsync<TmdbGenreResponse>(TmdbConf.BaseUrl,
                                                           "genre/movie/list",
                                                           BuildHeaders(),
                                                           new Dictionary<string, string> { { "language", language } },
                                                           managedCancellationToken.Token);
    }

    public async Task<TmdbMovieResponse?> GetMovieDetailsAsync(int movieId,
                                                               string language = "en",
                                                               string? appendToResponse = null)
    {
        var query = new Dictionary<string, string> { { "language", language } };
        if (!string.IsNullOrWhiteSpace(appendToResponse))
            query["append_to_response"] = appendToResponse;

        return await apiClient.GetAsync<TmdbMovieResponse>(TmdbConf.BaseUrl,
                                                           $"movie/{movieId}",
                                                           BuildHeaders(),
                                                           query,
                                                           managedCancellationToken.Token);
    }

    public async Task<TmdbSeriesResponse?> GetSeriesDetailsAsync(int seriesId, string language = "en", string? appendToResponse = null)
    {
        var query = new Dictionary<string, string> { { "language", language } };
        if (!string.IsNullOrWhiteSpace(appendToResponse))
            query["append_to_response"] = appendToResponse;

        return await apiClient.GetAsync<TmdbSeriesResponse>(TmdbConf.BaseUrl,
                                                            $"tv/{seriesId}",
                                                            BuildHeaders(),
                                                            query,
                                                            managedCancellationToken.Token);
    }
}
