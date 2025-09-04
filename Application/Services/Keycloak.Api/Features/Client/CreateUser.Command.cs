namespace Keycloak.Api.Features.Client;

public record KeycloakClientCreateUserCommand
(
    string UserName,
    string Email,
    IEnumerable<string> Groups,
    string FirstName,
    string LastName,
    bool IsActive
) : ICommand<KeycloakClientCreateUserResult>;

public record KeycloakClientCreateUserResult(bool IsSuccess);