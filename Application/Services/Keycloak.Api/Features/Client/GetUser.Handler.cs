namespace Keycloak.Api.Features.Client;

internal class KeycloakClientGetUserQueryHandler(IHttpClientFactory httpClientFactory,
                                                 ISender sender,
                                                 IAppSettingsKeyManagement appSettingsKeyManagement)
    : IQueryHandler<KeycloakClientGetUserQuery, List<KeycloakClientGetUserResult>>
{
    public async Task<List<KeycloakClientGetUserResult>> Handle(KeycloakClientGetUserQuery query, CancellationToken cancellationToken)
    {
        // get keycloak admin token
        var login = await sender.Send(new KeycloakAdminLoginCommand(), cancellationToken);

        var options = appSettingsKeyManagement.KeycloakOptions;
        var clientRealm = options!.Realms["Client"];
        var url = $"{options.EndPoints.BaseAddress}/admin/realms/{clientRealm.Name}/users?exact=true&email={query.Email}";
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

        return JsonSerializerHandler.Deserialize<List<KeycloakClientGetUserResult>>(json);
    }
}