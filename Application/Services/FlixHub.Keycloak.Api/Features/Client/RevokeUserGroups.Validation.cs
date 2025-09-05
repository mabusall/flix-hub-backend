namespace FlixHub.Keycloak.Api.Features.Client;

public class KeycloakClientRevokeUserGroupsCommandValidator : AbstractValidator<KeycloakClientRevokeUserGroupsCommand>
{
    public KeycloakClientRevokeUserGroupsCommandValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
        RuleFor(r => r.GroupIds)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
    }
}