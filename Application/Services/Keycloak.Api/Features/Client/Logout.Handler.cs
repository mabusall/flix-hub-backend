namespace Keycloak.Api.Features.Client;

internal class KeycloakClientLogoutCommandHandler(IHttpClientFactory httpClientFactory,
                                                  IAppSettingsKeyManagement appSettingsKeyManagement)
    : ICommandHandler<KeycloakClientLogoutCommand, KeycloakClientLogoutResult>
{
    public async Task<KeycloakClientLogoutResult> Handle(KeycloakClientLogoutCommand command, CancellationToken cancellationToken)
    {
        var options = appSettingsKeyManagement.KeycloakOptions;
        var clientRealm = options!.Realms["Client"];
        var url = string.Format($"{options.EndPoints.BaseAddress}{options.EndPoints.LogoutUser}",
            clientRealm.Name);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
        using var httpClient = httpClientFactory.CreateClient();
        using var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = clientRealm.Client,
            ["client_secret"] = clientRealm.Secret.Decrypt(),
            ["refresh_token"] = command.RefreshToken,
        });

        httpClient.BaseAddress = new Uri(url);
        httpRequest.Content = content;

        // send the request
        var response = await httpClient.SendAsync(httpRequest, cancellationToken);

        // validate the response
        response.EnsureSuccessStatusCode();

        // cast the result
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        return new KeycloakClientLogoutResult(true);
    }
}