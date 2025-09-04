namespace Keycloak.Api.Features.Client;

public class KeycloakClientGetUserSessionsQueryValidator : AbstractValidator<KeycloakClientGetUserSessionsQuery>
{
    public KeycloakClientGetUserSessionsQueryValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
    }
}