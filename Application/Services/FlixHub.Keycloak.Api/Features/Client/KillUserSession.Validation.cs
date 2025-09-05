namespace FlixHub.Keycloak.Api.Features.Client;

public class KeycloakClientKillUserSessionCommandValidator : AbstractValidator<KeycloakClientKillUserSessionCommand>
{
    public KeycloakClientKillUserSessionCommandValidator()
    {
        RuleFor(r => r.SessionId)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
    }
}