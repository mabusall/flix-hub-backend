namespace FlixHub.Keycloak.Api.Features.Client;

public class KeycloakClientAssignUserGroupsCommandValidator : AbstractValidator<KeycloakClientAssignUserGroupsCommand>
{
    public KeycloakClientAssignUserGroupsCommandValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
        RuleFor(r => r.GroupIds)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
    }
}