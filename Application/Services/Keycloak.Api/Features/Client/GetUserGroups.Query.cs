namespace Keycloak.Api.Features.Client;

public record KeycloakClientGetUserGroupsQuery
(
    Guid Id
) : IQuery<List<KeycloakClientGetUserGroupsResult>>;

public record KeycloakClientGetUserGroupsResult
(
    Guid Id,
    string? Name
);