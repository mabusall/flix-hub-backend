namespace FlixHub.Keycloak.Api.Features.Client;

internal class KeycloakClientGetUserSessionsQueryHandler(IHttpClientFactory httpClientFactory,
                                                         ISender sender,
                                                         IAppSettingsKeyManagement appSettingsKeyManagement)
    : IQueryHandler<KeycloakClientGetUserSessionsQuery, List<KeycloakClientGetUserSessionsResult>>
{
    public async Task<List<KeycloakClientGetUserSessionsResult>> Handle(KeycloakClientGetUserSessionsQuery query, CancellationToken cancellationToken)
    {
        // get user info
        var userInfo = (await sender.Send(new KeycloakClientGetUserQuery(query.Email), cancellationToken))
            .FirstOrDefault()
            ?? throw new BadRequestException("User not found");

        // get keycloak admin token
        var login = await sender.Send(new KeycloakAdminLoginCommand(), cancellationToken);

        var options = appSettingsKeyManagement.KeycloakOptions;
        var clientRealm = options!.Realms["Client"];
        var url = $"{options.EndPoints.BaseAddress}/admin/realms/{clientRealm.Name}/users/{userInfo.Id}/sessions";
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
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

        return json.Deserialize<List<KeycloakClientGetUserSessionsResult>>();
    }
}