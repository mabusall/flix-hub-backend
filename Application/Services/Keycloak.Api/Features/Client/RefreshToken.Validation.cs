namespace Keycloak.Api.Features.Client;

public class KeycloakClientRefreshTokenCommandValidator : AbstractValidator<KeycloakClientRefreshTokenCommand>
{
    public KeycloakClientRefreshTokenCommandValidator()
    {
        RuleFor(r => r.RefreshToken)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
    }
}