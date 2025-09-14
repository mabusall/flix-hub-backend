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

    public async Task<DiscoverResponse> GetDiscoverAsync(string? language = "en-US",
                                                         Dictionary<string, string> query = default!,
                                                         int? page = null)
    {
        if (!string.IsNullOrEmpty(language))
            query["language"] = language;
        if (page is not null)
            query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<DiscoverResponse>(TmdbConf.BaseUrl,
                                                            $"discover/tv",
                                                            BuildHeaders(),
                                                            query,
                                                            managedCancellationToken.Token);
    }

    public async Task<SearchResponse> GetSearchAsync(string? language = "en-US",
                                                     Dictionary<string, string> query = default!,
                                                     int? page = null)
    {
        if (!string.IsNullOrEmpty(language))
            query["language"] = language;
        if (page is not null)
            query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<SearchResponse>(TmdbConf.BaseUrl,
                                                          $"search/tv",
                                                          BuildHeaders(),
                                                          query,
                                                          managedCancellationToken.Token);
    }

    public async Task<TvDetailsResponse> GetDetailsAsync(int tvId)
    {
        return await apiClient.GetAsync<TvDetailsResponse>(TmdbConf.BaseUrl,
                                                          $"tv/{tvId}",
                                                          BuildHeaders(),
                                                          null,
                                                          managedCancellationToken.Token);
    }

    public async Task<TmdbExternalIdsResponse> GetExternalIdsAsync(int tvId)
    {
        return await apiClient.GetAsync<TmdbExternalIdsResponse>(TmdbConf.BaseUrl,
                                                               $"tv/{tvId}/external_ids",
                                                               BuildHeaders(),
                                                               null,
                                                               managedCancellationToken.Token);
    }

    public async Task<TmdbCreditsResponse> GetCreditsAsync(int tvId)
    {
        return await apiClient.GetAsync<TmdbCreditsResponse>(TmdbConf.BaseUrl,
                                                           $"tv/{tvId}/credits",
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

    public async Task<TmdbImagesResponse> GetImagesAsync(int tvId)
    {
        return await apiClient.GetAsync<TmdbImagesResponse>(TmdbConf.BaseUrl,
                                                            $"tv/{tvId}/images",
                                                            BuildHeaders(),
                                                            null,
                                                            managedCancellationToken.Token);
    }

    public async Task<TvKeywordsResponse> GetKeywordsAsync(int tvId)
    {
        return await apiClient.GetAsync<TvKeywordsResponse>(TmdbConf.BaseUrl,
                                                            $"tv/{tvId}/keywords",
                                                            BuildHeaders(),
                                                            null,
                                                            managedCancellationToken.Token);
    }

    public async Task<TmdbVideosResponse> GetVideosAsync(int tvId)
    {
        return await apiClient.GetAsync<TmdbVideosResponse>(TmdbConf.BaseUrl,
                                                            $"tv/{tvId}/videos",
                                                            BuildHeaders(),
                                                            null,
                                                            managedCancellationToken.Token);
    }

    public async Task<TvSeasonResponse> GetSeasonDetailsAsync(int tvId, int seasonNumber)
    {
        return await apiClient.GetAsync<TvSeasonResponse>(TmdbConf.BaseUrl,
                                                          $"tv/{tvId}/season/{seasonNumber}",
                                                          BuildHeaders(),
                                                          null,
                                                          managedCancellationToken.Token);
    }

    public async Task<TvEpisodeResponse> GetEpisodeDetailsAsync(int tvId,
                                                                int seasonNumber,
                                                                int episodeNumber)
    {
        return await apiClient.GetAsync<TvEpisodeResponse>(TmdbConf.BaseUrl,
                                                           $"tv/{tvId}/season/{seasonNumber}/episode/{episodeNumber}",
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

    public async Task<TvTrendingResponse> GetTrendingAsync(string timeWindow="week")
    {
        return await apiClient.GetAsync<TvTrendingResponse>(TmdbConf.BaseUrl,
                                                            $"trending/tv/{timeWindow}",
                                                            BuildHeaders(),
                                                            null,
                                                            managedCancellationToken.Token);
    }
}