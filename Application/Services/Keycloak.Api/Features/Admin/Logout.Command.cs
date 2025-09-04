namespace Keycloak.Api.Features.Admin;

public record KeycloakAdminLogoutCommand(string RefreshToken) : ICommand<KeycloakAdminLogoutResult>;

public record KeycloakAdminLogoutResult(bool IsSuccess);