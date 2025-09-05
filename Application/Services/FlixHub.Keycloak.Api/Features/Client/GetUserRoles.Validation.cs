namespace FlixHub.Keycloak.Api.Features.Client;

public class KeycloakClientGetUserRolesQueryValidator : AbstractValidator<KeycloakClientGetUserRolesQuery>
{
    public KeycloakClientGetUserRolesQueryValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
    }
}