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

    public async Task<ChangesResponse> ChangesAsync(string? language = null, int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page.HasValue) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<ChangesResponse>(
            TmdbConf.BaseUrl,
            $""movie/changes"",
            BuildHeaders(),
            query,
            managedCancellationToken.Token
        );
    }

    public async Task<PopularResponse> PopularAsync(string? language = null, int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page.HasValue) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<PopularResponse>(
            TmdbConf.BaseUrl,
            $""movie/popular"",
            BuildHeaders(),
            query,
            managedCancellationToken.Token
        );
    }

    public async Task<TopRatedResponse> TopRatedAsync(string? language = null, int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page.HasValue) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<TopRatedResponse>(
            TmdbConf.BaseUrl,
            $""movie/top_rated"",
            BuildHeaders(),
            query,
            managedCancellationToken.Token
        );
    }

    public async Task<UpcomingResponse> UpcomingAsync(string? language = null, int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page.HasValue) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<UpcomingResponse>(
            TmdbConf.BaseUrl,
            $""movie/upcoming"",
            BuildHeaders(),
            query,
            managedCancellationToken.Token
        );
    }

    public async Task<ImagesResponse> ImagesAsync(string id, string? language = null, int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page.HasValue) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<ImagesResponse>(
            TmdbConf.BaseUrl,
            $""movie/{id}/images"",
            BuildHeaders(),
            query,
            managedCancellationToken.Token
        );
    }

    public async Task<KeywordResponse> KeywordAsync(string id, string? language = null, int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page.HasValue) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<KeywordResponse>(
            TmdbConf.BaseUrl,
            $""movie/{id}/keywords"",
            BuildHeaders(),
            query,
            managedCancellationToken.Token
        );
    }

    public async Task<VideosTrailersResponse> VideosTrailersAsync(string id, string? language = null, int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page.HasValue) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<VideosTrailersResponse>(
            TmdbConf.BaseUrl,
            $""movie/{id}/videos"",
            BuildHeaders(),
            query,
            managedCancellationToken.Token
        );
    }

    public async Task<TrendingResponse> TrendingAsync(string time_window, string? language = null, int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page.HasValue) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<TrendingResponse>(
            TmdbConf.BaseUrl,
            $""trending/movie/{time_window}"",
            BuildHeaders(),
            query,
            managedCancellationToken.Token
        );
    }

}