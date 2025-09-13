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

    public async Task<DiscoverResponse> DiscoverAsync(string? language = "en-US",
                                                      int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page is not null) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<DiscoverResponse>(TmdbConf.BaseUrl,
                                                          $"discover/tv",
                                                          BuildHeaders(),
                                                          query,
                                                          managedCancellationToken.Token);
    }

    public async Task<DiscoverResponse> SearchAsync(string? language = "en-US",
                                                    int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page is not null) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<DiscoverResponse>(TmdbConf.BaseUrl,
                                                          $"search/tv",
                                                          BuildHeaders(),
                                                          query,
                                                          managedCancellationToken.Token);
    }

    public async Task<TrendingResponse> TrendingAsync(string time_window,
                                                      string? language = "en-US",
                                                      int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page is not null) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<TrendingResponse>(TmdbConf.BaseUrl,
                                                          $"trending/tv/{time_window}",
                                                          BuildHeaders(),
                                                          query,
                                                          managedCancellationToken.Token);
    }

    public async Task<ChangesResponse> ChangesAsync(string? language = "en-US",
                                                    int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page is not null) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<ChangesResponse>(
            TmdbConf.BaseUrl,
            $""tv/changes"",
            BuildHeaders(),
            query,
            managedCancellationToken.Token
        );
    }

    public async Task<DetailsResponse> DetailsAsync(string id,
                                                    string? language = "en-US",
                                                    int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page is not null) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<DetailsResponse>(
            TmdbConf.BaseUrl,
            $""tv/{id}"",
            BuildHeaders(),
            query,
            managedCancellationToken.Token
        );
    }

    public async Task<CreditsResponse> CreditsAsync(string id,
                                                    string? language = "en-US",
                                                    int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page is not null) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<CreditsResponse>(
            TmdbConf.BaseUrl,
            $""tv/{id}/credits"",
            BuildHeaders(),
            query,
            managedCancellationToken.Token
        );
    }

    public async Task<ExternalIDsResponse> ExternalIDsAsync(string id,
                                                            string? language = "en-US",
                                                            int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page is not null) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<ExternalIDsResponse>(
            TmdbConf.BaseUrl,
            $""tv/{id}/external_ids"",
            BuildHeaders(),
            query,
            managedCancellationToken.Token
        );
    }

    public async Task<ImagesResponse> ImagesAsync(string id,
                                                  string? language = "en-US",
                                                  int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page is not null) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<ImagesResponse>(
            TmdbConf.BaseUrl,
            $""tv/{id}/images"",
            BuildHeaders(),
            query,
            managedCancellationToken.Token
        );
    }

    public async Task<KeywordResponse> KeywordAsync(string id,
                                                    string? language = "en-US",
                                                    int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page is not null) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<KeywordResponse>(
            TmdbConf.BaseUrl,
            $""tv/{id}/keywords"",
            BuildHeaders(),
            query,
            managedCancellationToken.Token
        );
    }

    public async Task<SeasonDetailsResponse> SeasonDetailsAsync(string id,
                                                                string season_number,
                                                                string? language = "en-US",
                                                                int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page is not null) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<SeasonDetailsResponse>(
            TmdbConf.BaseUrl,
            $""tv/{id}/season/{season_number}"",
            BuildHeaders(),
            query,
            managedCancellationToken.Token
        );
    }

    public async Task<EpisodeDetailsResponse> EpisodeDetailsAsync(string id,
                                                                  string season_number,
                                                                  string episode_number,
                                                                  string? language = "en-US",
                                                                  int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page is not null) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<EpisodeDetailsResponse>(
            TmdbConf.BaseUrl,
            $""tv/{id}/season/{season_number}/episode/{episode_number}"",
            BuildHeaders(),
            query,
            managedCancellationToken.Token
        );
    }

    public async Task<VideosTrailersResponse> VideosTrailersAsync(string id,
                                                                  string? language = "en-US",
                                                                  int? page = null)
    {
        var query = new Dictionary<string,string>();
        if (!string.IsNullOrEmpty(language)) query["language"] = language;
        if (page is not null) query["page"] = page.Value.ToString();

        return await apiClient.GetAsync<VideosTrailersResponse>(
            TmdbConf.BaseUrl,
            $""tv/{id}/videos"",
            BuildHeaders(),
            query,
            managedCancellationToken.Token
        );
    }

}