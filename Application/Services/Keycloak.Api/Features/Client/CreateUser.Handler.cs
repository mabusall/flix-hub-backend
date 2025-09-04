namespace Keycloak.Api.Features.Client;

internal class KeycloakClientCreateUserCommandHandler(IHttpClientFactory httpClientFactory,
                                                      IAppSettingsKeyManagement appSettingsKeyManagement,
                                                      ISender sender)
    : ICommandHandler<KeycloakClientCreateUserCommand, KeycloakClientCreateUserResult>
{
    public async Task<KeycloakClientCreateUserResult> Handle(KeycloakClientCreateUserCommand command, CancellationToken cancellationToken)
    {
        // get keycloak admin token
        var login = await sender.Send(new KeycloakAdminLoginCommand(), cancellationToken);

        var createUserBody = new
        {
            username = command.UserName,
            email = command.Email,
            enabled = command.IsActive,
            firstName = command.FirstName,
            lastName = command.LastName,
            emailVerified = true,
            groups = command.Groups?.ToArray(),
        };

        // create body request
        var body = await JsonContent
            .Create(createUserBody)
            .ReadAsStringAsync(cancellationToken);

        // create user process
        var options = appSettingsKeyManagement.KeycloakOptions;
        var clientRealm = options!.Realms["Client"];
        var url = string.Format($"{options.EndPoints.BaseAddress}{options.EndPoints.CreateUser}",
            clientRealm.Name);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
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

        //... success
        return new KeycloakClientCreateUserResult(true);
    }
}