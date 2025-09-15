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

    public async Task<TmdbMediaListResponse> GetDiscoverAsync(string? language = "en-US",
                                                              Dictionary<string, string> query = default!,
                                                              int? page = null)
    {
        if (!string.IsNullOrEmpty(language))
            query["language"] = language;
        if (page is not null)
            query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<TmdbMediaListResponse>(TmdbConf.BaseUrl,
                                                                            $"discover/movie",
                                                                            BuildHeaders(),
                                                                            query,
                                                                            managedCancellationToken.Token);
    }

    public async Task<TmdbMediaListResponse> GetSearchAsync(string? language = "en-US",
                                                            Dictionary<string, string> query = default!,
                                                            int? page = null)
    {
        if (!string.IsNullOrEmpty(language))
            query["language"] = language;
        if (page is not null)
            query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<TmdbMediaListResponse>(TmdbConf.BaseUrl,
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

    public async Task<TmdbExternalIdsResponse> GetExternalIdsAsync(int id)
    {
        return await apiClient.GetAsync<TmdbExternalIdsResponse>(TmdbConf.BaseUrl,
                                                                 $"movie/{id}/external_ids",
                                                                 BuildHeaders(),
                                                                 null,
                                                                 managedCancellationToken.Token);
    }

    public async Task<TmdbCreditsResponse> GetCreditsAsync(int id)
    {
        return await apiClient.GetAsync<TmdbCreditsResponse>(TmdbConf.BaseUrl,
                                                             $"movie/{id}/credits",
                                                             BuildHeaders(),
                                                             null,
                                                             managedCancellationToken.Token);
    }

    public async Task<TmdbGenreResponse> GetGenresAsync()
    {
        return await apiClient.GetAsync<TmdbGenreResponse>(TmdbConf.BaseUrl,
                                                           $"genre/movie/list",
                                                           BuildHeaders(),
                                                           null,
                                                           managedCancellationToken.Token);
    }

    public async Task<TmdbImagesResponse> GetImagesAsync(int id)
    {
        return await apiClient.GetAsync<TmdbImagesResponse>(TmdbConf.BaseUrl,
                                                            $"movie/{id}/images",
                                                            BuildHeaders(),
                                                            null,
                                                            managedCancellationToken.Token);
    }

    public async Task<MovieKeywordsResponse> GetKeywordsAsync(int id)
    {
        return await apiClient.GetAsync<MovieKeywordsResponse>(TmdbConf.BaseUrl,
                                                               $"movie/{id}/keywords",
                                                               BuildHeaders(),
                                                               null,
                                                               managedCancellationToken.Token);
    }

    public async Task<TmdbVideosResponse> GetVideosAsync(int id)
    {
        return await apiClient.GetAsync<TmdbVideosResponse>(TmdbConf.BaseUrl,
                                                            $"movie/{id}/videos",
                                                            BuildHeaders(),
                                                            null,
                                                            managedCancellationToken.Token);
    }

    public async Task<MovieUpcomingResponse> GetUpcomingsAsync(string? language = "en-US",
                                                               int? page = null)
    {
        Dictionary<string, string> query = [];
        if (!string.IsNullOrEmpty(language))
            query["language"] = language;
        if (page is not null)
            query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<MovieUpcomingResponse>(TmdbConf.BaseUrl,
                                                               $"movie/upcoming",
                                                               BuildHeaders(),
                                                               query,
                                                               managedCancellationToken.Token);
    }

    public async Task<TmdbMediaListResponse> GetTrendingAsync(string timeWindow = "week", int? page = null)
    {
        Dictionary<string, string> query = [];
        if (page is not null)
            query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<TmdbMediaListResponse>(TmdbConf.BaseUrl,
                                                               $"trending/movie/{timeWindow}",
                                                               BuildHeaders(),
                                                               query,
                                                               managedCancellationToken.Token);
    }

    public async Task<TmdbMediaListResponse> GetTopRatedAsync(int? page = null)
    {
        Dictionary<string, string> query = [];
        if (page is not null)
            query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<TmdbMediaListResponse>(TmdbConf.BaseUrl,
                                                               $"movie/top_rated",
                                                               BuildHeaders(),
                                                               query,
                                                               managedCancellationToken.Token);
    }

    public async Task<TmdbMediaListResponse> GetPopularAsync(int? page = null)
    {
        Dictionary<string, string> query = [];
        if (page is not null)
            query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<TmdbMediaListResponse>(TmdbConf.BaseUrl,
                                                               $"movie/popular",
                                                               BuildHeaders(),
                                                               query,
                                                               managedCancellationToken.Token);
    }

    public async Task<TmdbChangesResponse> GetChangesAsync(string? language = "en-US",
                                                           Dictionary<string, string> query = default!,
                                                           int? page = null)
    {
        if (!string.IsNullOrEmpty(language))
            query["language"] = language;
        if (page is not null)
            query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<TmdbChangesResponse>(TmdbConf.BaseUrl,
                                                           $"movie/changes",
                                                           BuildHeaders(),
                                                           query,
                                                           managedCancellationToken.Token);
    }
}