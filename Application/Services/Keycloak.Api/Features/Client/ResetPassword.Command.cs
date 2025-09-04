namespace Keycloak.Api.Features.Client;

public record KeycloakClientResetPasswordCommand
(
    string Email,
    string Password
) : ICommand<KeycloakClientResetPasswordResult>;

public record KeycloakClientResetPasswordResult(bool IsSuccess);