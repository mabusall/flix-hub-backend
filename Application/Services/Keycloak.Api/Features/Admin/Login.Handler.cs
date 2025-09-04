namespace Keycloak.Api.Features.Admin;

internal class KeycloakAdminLoginCommandHandler(IHttpClientFactory httpClientFactory,
                                                IAppSettingsKeyManagement appSettingsKeyManagement)
    : ICommandHandler<KeycloakAdminLoginCommand, KeycloakAdminLoginResult>
{
    public async Task<KeycloakAdminLoginResult> Handle(KeycloakAdminLoginCommand command, CancellationToken cancellationToken)
    {
        var options = appSettingsKeyManagement.KeycloakOptions;
        var adminRealm = options!.Realms["Admin"];
        var url = string.Format($"{options.EndPoints.BaseAddress}{options.EndPoints.TokenExchange}",
            adminRealm.Name);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
        using var httpClient = httpClientFactory.CreateClient();
        using var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"] = adminRealm.Client,
            ["username"] = adminRealm.UserName.Decrypt(),
            ["password"] = adminRealm.Password.Decrypt(),
        });

        httpClient.BaseAddress = new Uri(url);
        httpRequest.Content = content;

        // send the request
        var response = await httpClient.SendAsync(httpRequest, cancellationToken);

        // validate the response
        response.EnsureSuccessStatusCode();

        // cast the result
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        return JsonSerializerHandler.Deserialize<KeycloakAdminLoginResult>(json);
    }
}