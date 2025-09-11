namespace FlixHub.Core.Api.Services;

internal class TmdbService(IApiClient apiClient,
                           IAppSettingsKeyManagement appSettings,
                           IManagedCancellationToken managedCancellationToken)
{
    public async Task<TmdbGenreResponse?> GetGenresAsync(string language = "en")
    {
        var tmdpConf = appSettings.IntegrationApisOptions.Apis["TMDB"];

        var headers = new Dictionary<string, string>
        {
            { "Authorization", $"Bearer {tmdpConf.Token.Decrypt()}" }
        };

        return await apiClient.GetAsync<TmdbGenreResponse>(tmdpConf.BaseUrl,
                                                           "genre/movie/list",
                                                           headers,
                                                           new Dictionary<string, string> { { "language", language } },
                                                           managedCancellationToken.Token);
    }
}

public record TmdbGenreResponse(IList<TmdbGenre> Genres);
public record TmdbGenre(int Id, string Name);

