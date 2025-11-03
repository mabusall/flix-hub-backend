namespace FlixHub.Core.Api.Services;

public sealed class OmdbQuotaExceededException(string message) : Exception(message) { }

internal sealed class OmdbService(IApiClient apiClient,
                                  IAppSettingsKeyManagement appSettings,
                                  IManagedCancellationToken appToken,
                                  ITokenRing tokenRing)
{
    public IntegrationApi OmdbConf { get; } = appSettings.IntegrationApisOptions.Apis["OMDB"];

    public async Task<OmdbImdbDetailsResponse> GetImdbDetailsAsync(string imdbId)
    {
        var tried = 0;
        var maxTries = OmdbConf.Tokens.Count; // try each token at most once
        OmdbImdbDetailsResponse response = null!;

        while (tried < maxTries)
        {
            tried++;

            var requestFailed = false;
            var token = tokenRing.Current; // current winner
            var query = new Dictionary<string, string>
        {
            { "i", imdbId },
            { "apikey", token }
        };

            try
            {
                // Prefer a method that gives you both StatusCode and Content.
                // If your IApiClient only returns T, add a RawGetAsync that returns HttpResponseMessage as well.
                response = await apiClient.GetAsync<OmdbImdbDetailsResponse>(OmdbConf.BaseUrl,
                                                                             null,
                                                                             null,
                                                                             query,
                                                                             appToken.Token);
            }
            catch
            {
                // 401 Unauthorized, 403 Forbidden (some providers use it for bad/expired key), 429 TooManyRequests (quota on this key)
                requestFailed = true;
            }

            // Decide which failures should rotate the token:
            if (!requestFailed)
                break; // success

            // try next token
            tokenRing.Advance();
        }

        if (response is null)
            throw new OmdbQuotaExceededException("Reached max daily requests for OMDB API.");

        return response;
    }
}