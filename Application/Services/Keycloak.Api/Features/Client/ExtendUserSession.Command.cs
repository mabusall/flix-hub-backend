namespace Keycloak.Api.Features.Client;

public record KeycloakClientExtendUserSessionCommand
(
    string RefreshToken
) : ICommand<KeycloakClientExtendUserSessionResult>;

public record KeycloakClientExtendUserSessionResult
(
    bool IsSuccess
);