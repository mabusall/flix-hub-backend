namespace FlixHub.Keycloak.Api.Features.Client;

public record KeycloakClientGetUserRolesQuery
(
    Guid Id
) : IQuery<List<KeycloakClientGetUserRolesResult>>;

public record KeycloakClientGetUserRolesResult
(
    Guid Id,
    string? Name,
    string? Description
);