namespace FlixHub.Keycloak.Api.Features.Client;

public record KeycloakClientDeleteUserCommand
(
    Guid Id
) : ICommand<KeycloakClientDeleteUserResult>;

public record KeycloakClientDeleteUserResult(bool IsSuccess);