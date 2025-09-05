namespace FlixHub.Keycloak.Api.Features.Client;

internal class KeycloakClientGetUserIntrospectQueryHandler(IHttpClientFactory httpClientFactory,
                                                           IAppSettingsKeyManagement appSettingsKeyManagement)
    : IQueryHandler<KeycloakClientGetUserIntrospectQuery, KeycloakClientGetUserIntrospectResult>
{
    public async Task<KeycloakClientGetUserIntrospectResult> Handle(KeycloakClientGetUserIntrospectQuery query, CancellationToken cancellationToken)
    {
        var options = appSettingsKeyManagement.KeycloakOptions;
        var clientRealm = options!.Realms["Client"];
        var url = string.Format($"{options.EndPoints.BaseAddress}{options!.EndPoints.Introspect}", clientRealm.Name);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
        using var httpClient = httpClientFactory.CreateClient();
        using var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["token"] = query.AccessToken,
            ["client_id"] = clientRealm.Client,
            ["client_secret"] = clientRealm.Secret.Decrypt(),
        });

        httpRequest.Content = content;
        httpClient.BaseAddress = new Uri(url);

        // send the request
        var response = await httpClient.SendAsync(httpRequest, cancellationToken);

        // cast the result
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        // validate the response
        if (!response.IsSuccessStatusCode)
            throw new BadRequestException(json);

        return json.Deserialize<KeycloakClientGetUserIntrospectResult>();
    }
}