namespace Keycloak.Api.Features.Client;

public record KeycloakClientLogoutCommand(string RefreshToken) : ICommand<KeycloakClientLogoutResult>;

public record KeycloakClientLogoutResult(bool IsSuccess);