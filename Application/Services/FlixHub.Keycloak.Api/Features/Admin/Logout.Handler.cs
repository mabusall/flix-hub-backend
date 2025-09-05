namespace FlixHub.Keycloak.Api.Features.Admin;

internal class KeycloakAdminLogoutCommandHandler(IHttpClientFactory httpClientFactory,
                                                 IAppSettingsKeyManagement appSettingsKeyManagement)
    : ICommandHandler<KeycloakAdminLogoutCommand, KeycloakAdminLogoutResult>
{
    public async Task<KeycloakAdminLogoutResult> Handle(KeycloakAdminLogoutCommand command, CancellationToken cancellationToken)
    {
        var options = appSettingsKeyManagement.KeycloakOptions;
        var adminRealm = options!.Realms["Admin"];
        var url = string.Format($"{options.EndPoints.BaseAddress}{options.EndPoints.LogoutUser}",
            adminRealm.Name);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
        using var httpClient = httpClientFactory.CreateClient();
        using var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = adminRealm.Client,
            ["refresh_token"] = command.RefreshToken,
        });

        httpClient.BaseAddress = new Uri(url);
        httpRequest.Content = content;

        // send the request
        var response = await httpClient.SendAsync(httpRequest, cancellationToken);

        // validate the response
        response.EnsureSuccessStatusCode();

        return new KeycloakAdminLogoutResult(true);
    }
}