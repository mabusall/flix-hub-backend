namespace FlixHub.Keycloak.Api.Features.Client;

public record KeycloakClientKillUserSessionCommand
(
    Guid SessionId
) : ICommand<KeycloakClientKillUserSessionResult>;

public record KeycloakClientKillUserSessionResult
(
    bool IsSuccess
);