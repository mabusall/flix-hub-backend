namespace FlixHub.Keycloak.Api.Features.Client;

internal class KeycloakClientResetPasswordCommandHandler(IHttpClientFactory httpClientFactory,
                                                         ISender sender,
                                                         IAppSettingsKeyManagement appSettingsKeyManagement)
    : ICommandHandler<KeycloakClientResetPasswordCommand, KeycloakClientResetPasswordResult>
{
    public async Task<KeycloakClientResetPasswordResult> Handle(KeycloakClientResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var userInfo = (await sender.Send
            (new KeycloakClientGetUserQuery(command.Email), cancellationToken))
            .FirstOrDefault()
            ??
            throw new BadRequestException(ErrorMessageResources.UserManagement_EmailNotFound);

        // get keycloak admin token
        var login = await sender.Send(new KeycloakAdminLoginCommand(), cancellationToken);

        var resetPasswordBody = new
        {
            type = "password",
            temporary = false,
            value = command.Password,
        };

        // create body request
        var body = await JsonContent
            .Create(resetPasswordBody)
            .ReadAsStringAsync(cancellationToken);

        var options = appSettingsKeyManagement.KeycloakOptions;
        var clientRealm = options!.Realms["Client"];
        var url = string.Format($"{options.EndPoints.BaseAddress}{options.EndPoints.CreateUser}/{userInfo.Id}/reset-password",
            clientRealm.Name);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Put, url);
        using var httpClient = httpClientFactory.CreateClient();
        using var content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json);

        httpClient.AttachBearerAuthentication(login.AccessToken);
        httpClient.BaseAddress = new Uri(url);
        httpRequest.Content = content;

        // send the request
        var response = await httpClient.SendAsync(httpRequest, cancellationToken);

        // logout admin session
        await sender.Send(new KeycloakAdminLogoutCommand
        (
            login.RefreshToken!
        ), cancellationToken);

        // validate the response
        response.EnsureSuccessStatusCode();

        return new KeycloakClientResetPasswordResult(true);
    }
}