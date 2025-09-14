namespace FlixHub.Core.Api.Services;

internal sealed class TmdbTvService(IApiClient apiClient,
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
                                                                            $"discover/tv",
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
                                                                            $"search/tv",
                                                                            BuildHeaders(),
                                                                            query,
                                                                            managedCancellationToken.Token);
    }

    public async Task<TvDetailsResponse> GetDetailsAsync(int id)
    {
        return await apiClient.GetAsync<TvDetailsResponse>(TmdbConf.BaseUrl,
                                                          $"tv/{id}",
                                                          BuildHeaders(),
                                                          null,
                                                          managedCancellationToken.Token);
    }

    public async Task<TmdbExternalIdsResponse> GetExternalIdsAsync(int id)
    {
        return await apiClient.GetAsync<TmdbExternalIdsResponse>(TmdbConf.BaseUrl,
                                                               $"tv/{id}/external_ids",
                                                               BuildHeaders(),
                                                               null,
                                                               managedCancellationToken.Token);
    }

    public async Task<TmdbCreditsResponse> GetCreditsAsync(int id)
    {
        return await apiClient.GetAsync<TmdbCreditsResponse>(TmdbConf.BaseUrl,
                                                           $"tv/{id}/credits",
                                                           BuildHeaders(),
                                                           null,
                                                           managedCancellationToken.Token);
    }

    public async Task<TmdbGenreResponse> GetGenresAsync()
    {
        return await apiClient.GetAsync<TmdbGenreResponse>(TmdbConf.BaseUrl,
                                                           $"genre/tv/list",
                                                           BuildHeaders(),
                                                           null,
                                                           managedCancellationToken.Token);
    }

    public async Task<TmdbImagesResponse> GetImagesAsync(int id)
    {
        return await apiClient.GetAsync<TmdbImagesResponse>(TmdbConf.BaseUrl,
                                                            $"tv/{id}/images",
                                                            BuildHeaders(),
                                                            null,
                                                            managedCancellationToken.Token);
    }

    public async Task<TvKeywordsResponse> GetKeywordsAsync(int id)
    {
        return await apiClient.GetAsync<TvKeywordsResponse>(TmdbConf.BaseUrl,
                                                            $"tv/{id}/keywords",
                                                            BuildHeaders(),
                                                            null,
                                                            managedCancellationToken.Token);
    }

    public async Task<TmdbVideosResponse> GetVideosAsync(int id)
    {
        return await apiClient.GetAsync<TmdbVideosResponse>(TmdbConf.BaseUrl,
                                                            $"tv/{id}/videos",
                                                            BuildHeaders(),
                                                            null,
                                                            managedCancellationToken.Token);
    }

    public async Task<TvSeasonResponse> GetSeasonDetailsAsync(int id, int seasonNumber)
    {
        return await apiClient.GetAsync<TvSeasonResponse>(TmdbConf.BaseUrl,
                                                          $"tv/{id}/season/{seasonNumber}",
                                                          BuildHeaders(),
                                                          null,
                                                          managedCancellationToken.Token);
    }

    public async Task<TvEpisodeResponse> GetEpisodeDetailsAsync(int id,
                                                                int seasonNumber,
                                                                int episodeNumber)
    {
        return await apiClient.GetAsync<TvEpisodeResponse>(TmdbConf.BaseUrl,
                                                           $"tv/{id}/season/{seasonNumber}/episode/{episodeNumber}",
                                                           BuildHeaders(),
                                                           null,
                                                           managedCancellationToken.Token);
    }

    public async Task<TvChangesResponse> GetChangesAsync(string? language = "en-US",
                                                         Dictionary<string, string> query = default!,
                                                         int? page = null)
    {
        if (!string.IsNullOrEmpty(language))
            query["language"] = language;
        if (page is not null)
            query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<TvChangesResponse>(TmdbConf.BaseUrl,
                                                           $"tv/changes",
                                                           BuildHeaders(),
                                                           query,
                                                           managedCancellationToken.Token);
    }

    public async Task<TmdbDiscoverSearchTrendingResponse> GetTrendingAsync(string timeWindow="week")
    {
        return await apiClient.GetAsync<TmdbDiscoverSearchTrendingResponse>(TmdbConf.BaseUrl,
                                                                            $"trending/tv/{timeWindow}",
                                                                            BuildHeaders(),
                                                                            null,
                                                                            managedCancellationToken.Token);
    }
}