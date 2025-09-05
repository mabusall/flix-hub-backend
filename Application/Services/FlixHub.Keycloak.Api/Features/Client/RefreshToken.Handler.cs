namespace FlixHub.Keycloak.Api.Features.Client;

internal class KeycloakClientRefreshTokenCommandHandler(IHttpClientFactory httpClientFactory,
                                                        IAppSettingsKeyManagement appSettingsKeyManagement)
    : ICommandHandler<KeycloakClientRefreshTokenCommand, KeycloakClientRefreshTokenResult>
{
    public async Task<KeycloakClientRefreshTokenResult> Handle(KeycloakClientRefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var options = appSettingsKeyManagement.KeycloakOptions;
        var clientRealm = options!.Realms["Client"];
        var url = string.Format($"{options.EndPoints.BaseAddress}{options.EndPoints.TokenExchange}",
            clientRealm.Name);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
        using var httpClient = httpClientFactory.CreateClient();
        using var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["refresh_token"] = command.RefreshToken,
            ["client_id"] = clientRealm.Client,
            ["client_secret"] = clientRealm.Secret.Decrypt(),
        });

        httpClient.BaseAddress = new Uri(url);
        httpRequest.Content = content;

        // send the request
        var response = await httpClient.SendAsync(httpRequest, cancellationToken);

        // validate the response
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
            throw new UnauthorizedException("Invalid refresh token");
        }

        // cast the result
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        return json.Deserialize<KeycloakClientRefreshTokenResult>();
    }
}