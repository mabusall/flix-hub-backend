namespace FlixHub.Keycloak.Api.Features.Client;

public class KeycloakClientLogoutCommandValidator : AbstractValidator<KeycloakClientLogoutCommand>
{
    public KeycloakClientLogoutCommandValidator()
    {
        RuleFor(r => r.RefreshToken)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
    }
}