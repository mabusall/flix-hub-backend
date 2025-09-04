namespace Keycloak.Api.Features.Admin;

internal class KeycloakAdminClientsQueryHandler(IHttpClientFactory httpClientFactory,
                                                ISender sender,
                                                IAppSettingsKeyManagement appSettingsKeyManagement)
    : IQueryHandler<KeycloakAdminClientsQuery, List<KeycloakAdminClientsResult>>
{
    public async Task<List<KeycloakAdminClientsResult>> Handle(KeycloakAdminClientsQuery query, CancellationToken cancellationToken)
    {
        // get keycloak admin token
        var login = await sender.Send(new KeycloakAdminLoginCommand(), cancellationToken);
        var options = appSettingsKeyManagement.KeycloakOptions;
        var adminRealm = options!.Realms["Admin"];
        var url = $"{options.EndPoints.BaseAddress}/admin/realms/{adminRealm.Name}/clients";
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
        using var httpClient = httpClientFactory.CreateClient();

        httpClient.BaseAddress = new Uri(url);
        httpClient.AttachBearerAuthentication(login.AccessToken);

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

        return JsonSerializerHandler.Deserialize<List<KeycloakAdminClientsResult>>(json);
    }
}