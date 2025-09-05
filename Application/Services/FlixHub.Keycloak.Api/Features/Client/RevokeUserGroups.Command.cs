namespace FlixHub.Keycloak.Api.Features.Client;

public record KeycloakClientRevokeUserGroupsCommand
    : ICommand<KeycloakClientRevokeUserGroupsResult>
{
    public Guid Id { get; set; }
    public IEnumerable<Guid>? GroupIds { get; set; }
}

public record KeycloakClientRevokeUserGroupsResult
(
    bool IsSuccess
);