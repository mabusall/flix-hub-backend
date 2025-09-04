namespace Keycloak.Api.Features.Client;

public record KeycloakClientAssignUserGroupsCommand
    : ICommand<KeycloakClientAssignUserGroupsResult>
{
    public Guid Id { get; set; }
    public IEnumerable<Guid>? GroupIds { get; set; }
}

public record KeycloakClientAssignUserGroupsResult
(
    bool IsSuccess
);