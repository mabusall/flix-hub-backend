namespace Keycloak.Api.Features.Client;

public class KeycloakClientGetUserGroupsQueryValidator : AbstractValidator<KeycloakClientGetUserGroupsQuery>
{
    public KeycloakClientGetUserGroupsQueryValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
    }
}