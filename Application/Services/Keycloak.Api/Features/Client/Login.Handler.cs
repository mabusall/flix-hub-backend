namespace Keycloak.Api.Features.Client;

internal class KeycloakClientLoginCommandHandler(IHttpClientFactory httpClientFactory,
                                                IAppSettingsKeyManagement appSettingsKeyManagement)
    : ICommandHandler<KeycloakClientLoginCommand, KeycloakClientLoginResult>
{
    public async Task<KeycloakClientLoginResult> Handle(KeycloakClientLoginCommand command, CancellationToken cancellationToken)
    {
        var options = appSettingsKeyManagement.KeycloakOptions;
        var clientRealm = options!.Realms["Client"];
        var url = string.Format($"{options.EndPoints.BaseAddress}{options.EndPoints.TokenExchange}",
            clientRealm.Name);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
        using var httpClient = httpClientFactory.CreateClient();
        using var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"] = clientRealm.Client,
            ["client_secret"] = clientRealm.Secret.Decrypt(),
            ["username"] = command.Email,
            ["password"] = command.Password,
            ["scope"] = clientRealm.Scopes
        });

        httpClient.BaseAddress = new Uri(url);
        httpRequest.Content = content;

        // send the request
        var response = await httpClient.SendAsync(httpRequest, cancellationToken);

        // validate the response
        response.EnsureSuccessStatusCode();

        // cast the result
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        return JsonSerializerHandler.Deserialize<KeycloakClientLoginResult>(json);
    }
}