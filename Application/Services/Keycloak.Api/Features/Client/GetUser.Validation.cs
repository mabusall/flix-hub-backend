namespace Keycloak.Api.Features.Client;

public class KeycloakClientGetUserQueryValidator : AbstractValidator<KeycloakClientGetUserQuery>
{
    public KeycloakClientGetUserQueryValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
    }
}