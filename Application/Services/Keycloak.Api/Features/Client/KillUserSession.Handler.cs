namespace Keycloak.Api.Features.Client;

internal class KeycloakClientKillUserSessionCommandHandler(IHttpClientFactory httpClientFactory,
                                                           ISender sender,
                                                           IAppSettingsKeyManagement appSettingsKeyManagement)
    : ICommandHandler<KeycloakClientKillUserSessionCommand, KeycloakClientKillUserSessionResult>
{
    public async Task<KeycloakClientKillUserSessionResult> Handle(KeycloakClientKillUserSessionCommand command, CancellationToken cancellationToken)
    {
        // get keycloak admin token
        var login = await sender.Send(new KeycloakAdminLoginCommand(), cancellationToken);

        var options = appSettingsKeyManagement.KeycloakOptions;
        var clientRealm = options!.Realms["Client"];
        var url = $"{options.EndPoints.BaseAddress}/admin/realms/{clientRealm.Name}/sessions/{command.SessionId}";
        using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, url);
        using var httpClient = httpClientFactory.CreateClient();

        httpClient.AttachBearerAuthentication(login.AccessToken);
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

        return new KeycloakClientKillUserSessionResult(true);
    }
}