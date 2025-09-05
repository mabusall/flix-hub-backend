namespace FlixHub.Keycloak.Api.Features.Client;

public class KeycloakClientExtendUserSessionCommandValidator : AbstractValidator<KeycloakClientExtendUserSessionCommand>
{
    public KeycloakClientExtendUserSessionCommandValidator()
    {
        RuleFor(r => r.RefreshToken)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
    }
}