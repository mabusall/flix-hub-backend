namespace FlixHub.Keycloak.Api.Features.Client;

internal class KeycloakClientExtendUserSessionCommandHandler(IHttpClientFactory httpClientFactory,
                                                             ISender sender,
                                                             IAppSettingsKeyManagement appSettingsKeyManagement)
    : ICommandHandler<KeycloakClientExtendUserSessionCommand, KeycloakClientExtendUserSessionResult>
{
    public async Task<KeycloakClientExtendUserSessionResult> Handle(KeycloakClientExtendUserSessionCommand command, CancellationToken cancellationToken)
    {
        // get keycloak admin token
        var login = await sender.Send(new KeycloakAdminLoginCommand(), cancellationToken);

        var options = appSettingsKeyManagement.KeycloakOptions;
        var clientRealm = options!.Realms["Client"];
        var url = string.Format($"{options.EndPoints.BaseAddress}{options.EndPoints.TokenExchange}",
            clientRealm.Name);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
        using var httpClient = httpClientFactory.CreateClient();
        using var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = clientRealm.Client,
            ["client_secret"] = clientRealm.Secret.Decrypt(),
            ["refresh_token"] = command.RefreshToken,
            ["grant_type"] = "refresh_token",
        });

        httpClient.AttachBearerAuthentication(login.AccessToken);
        httpRequest.Content = content;
        httpClient.BaseAddress = new Uri(url);

        // send the request
        var response = await httpClient.SendAsync(httpRequest, cancellationToken);
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        // logout admin session
        await sender.Send(new KeycloakAdminLogoutCommand
        (
            login.RefreshToken!
        ), cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new BadRequestException(json);

        return new KeycloakClientExtendUserSessionResult(true);
    }
}