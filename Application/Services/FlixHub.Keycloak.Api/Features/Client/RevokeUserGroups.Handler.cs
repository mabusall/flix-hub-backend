namespace FlixHub.Keycloak.Api.Features.Client;

internal class KeycloakClientRevokeUserGroupsCommandHandler(IHttpClientFactory httpClientFactory,
                                                            ISender sender,
                                                            IAppSettingsKeyManagement appSettingsKeyManagement)
    : ICommandHandler<KeycloakClientRevokeUserGroupsCommand, KeycloakClientRevokeUserGroupsResult>
{
    public async Task<KeycloakClientRevokeUserGroupsResult> Handle(KeycloakClientRevokeUserGroupsCommand command, CancellationToken cancellationToken)
    {
        // get keycloak admin token
        var login = await sender.Send(new KeycloakAdminLoginCommand(), cancellationToken);

        var options = appSettingsKeyManagement.KeycloakOptions;
        var clientRealm = options!.Realms["Client"];

        // Declare a Func<Task<string>> inside the parent function
        Func<Task<string?>> inlineFunc = async () =>
        {
            foreach (var groupId in command.GroupIds!)
            {
                try
                {
                    var url = $"{options.EndPoints.BaseAddress}/admin/realms/{clientRealm.Name}/users/{command.Id}/groups/{groupId}";
                    using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, url);
                    using var httpClient = httpClientFactory.CreateClient();

                    httpClient.AttachBearerAuthentication(login.AccessToken);
                    httpClient.BaseAddress = new Uri(url);

                    // send the request
                    var response = await httpClient.SendAsync(httpRequest, cancellationToken);

                    // validate the response
                    if (!response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync(cancellationToken);
                    }
                }
                catch
                {
                    break;
                }
            }

            return null;
        };

        // Call the local async function and await the result
        var result = await inlineFunc();

        // logout admin session
        await sender.Send(new KeycloakAdminLogoutCommand
        (
            login.RefreshToken!
        ), cancellationToken);

        if (result is not null) throw new BadRequestException(result);

        return new KeycloakClientRevokeUserGroupsResult(true);
    }
}