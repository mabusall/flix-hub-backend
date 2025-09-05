namespace FlixHub.Keycloak.Api.Features.Client;

internal class KeycloakClientGetUserGroupsQueryHandler(IHttpClientFactory httpClientFactory,
                                                       ISender sender,
                                                       IAppSettingsKeyManagement appSettingsKeyManagement)
    : IQueryHandler<KeycloakClientGetUserGroupsQuery, List<KeycloakClientGetUserGroupsResult>>
{
    public async Task<List<KeycloakClientGetUserGroupsResult>> Handle(KeycloakClientGetUserGroupsQuery query, CancellationToken cancellationToken)
    {
        // get keycloak admin token
        var login = await sender.Send(new KeycloakAdminLoginCommand(), cancellationToken);

        var options = appSettingsKeyManagement.KeycloakOptions;
        var clientRealm = options!.Realms["Client"];
        var url = $"{options.EndPoints.BaseAddress}/admin/realms/{clientRealm.Name}/users/{query.Id}/groups?briefRepresentation=false";
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
        using var httpClient = httpClientFactory.CreateClient();

        httpClient.AttachBearerAuthentication(login.AccessToken);
        httpClient.BaseAddress = new Uri(url);

        // send the request
        var response = await httpClient.SendAsync(httpRequest, cancellationToken);

        // validate the response
        response.EnsureSuccessStatusCode();

        // cast the result
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        // logout admin session
        await sender.Send(new KeycloakAdminLogoutCommand
        (
            login.RefreshToken!
        ), cancellationToken);

        return json.Deserialize<List<KeycloakClientGetUserGroupsResult>>();
    }
}