namespace Keycloak.Api.Features.Client;

public class KeycloakClientGetUserIntrospectQueryValidator : AbstractValidator<KeycloakClientGetUserIntrospectQuery>
{
    public KeycloakClientGetUserIntrospectQueryValidator()
    {
        RuleFor(r => r.AccessToken)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
    }
}