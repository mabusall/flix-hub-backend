namespace Keycloak.Api.Features.Client;

public record KeycloakClientGetGroupsQuery : IQuery<List<KeycloakClientGetGroupsResult>>;

public record KeycloakClientGetGroupsResult
(
    Guid Id,
    string Name
);