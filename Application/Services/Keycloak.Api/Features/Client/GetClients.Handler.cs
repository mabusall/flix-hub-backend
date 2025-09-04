namespace Keycloak.Api.Features.Client;

internal class KeycloakClientGetClientsQueryHandler(IHttpClientFactory httpClientFactory,
                                                    ISender sender,
                                                    IAppSettingsKeyManagement appSettingsKeyManagement)
    : IQueryHandler<KeycloakClientGetClientsQuery, List<KeycloakClientGetClientsResult>>
{
    public async Task<List<KeycloakClientGetClientsResult>> Handle(KeycloakClientGetClientsQuery query, CancellationToken cancellationToken)
    {
        // get keycloak admin token
        var login = await sender.Send(new KeycloakAdminLoginCommand(), cancellationToken);
        var options = appSettingsKeyManagement.KeycloakOptions;
        var clientRealm = options!.Realms["Client"];
        var url = $"{options.EndPoints.BaseAddress}/admin/realms/{clientRealm.Name}/clients";
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

        return json.Deserialize<List<KeycloakClientGetClientsResult>>();
    }
}