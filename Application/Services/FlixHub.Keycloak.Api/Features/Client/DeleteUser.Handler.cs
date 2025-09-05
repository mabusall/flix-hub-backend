namespace FlixHub.Keycloak.Api.Features.Client;

internal class KeycloakClientDeleteUserCommandHandler(IHttpClientFactory httpClientFactory,
                                                      IAppSettingsKeyManagement appSettingsKeyManagement,
                                                      ISender sender)
    : ICommandHandler<KeycloakClientDeleteUserCommand, KeycloakClientDeleteUserResult>
{
    public async Task<KeycloakClientDeleteUserResult> Handle(KeycloakClientDeleteUserCommand command, CancellationToken cancellationToken)
    {
        // get keycloak admin token
        var login = await sender.Send(new KeycloakAdminLoginCommand(), cancellationToken);

        var options = appSettingsKeyManagement.KeycloakOptions;
        var clientRealm = options!.Realms["Client"];
        var url = $"{options.EndPoints.BaseAddress}/admin/realms/{clientRealm.Name}/users/{command.Id}";
        using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, url);
        using var httpClient = httpClientFactory.CreateClient();

        httpClient.AttachBearerAuthentication(login.AccessToken);
        httpClient.BaseAddress = new Uri(url);

        // send the request
        var response = await httpClient.SendAsync(httpRequest, cancellationToken);

        // logout admin session
        await sender.Send(new KeycloakAdminLogoutCommand
        (
            login.RefreshToken!
        ), cancellationToken);

        // validate the response
        response.EnsureSuccessStatusCode();

        // cast the result
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        return new KeycloakClientDeleteUserResult(true);
    }
}