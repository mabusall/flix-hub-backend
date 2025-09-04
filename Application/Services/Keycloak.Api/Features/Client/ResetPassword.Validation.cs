namespace Keycloak.Api.Features.Client;

public class KeycloakClientResetPasswordCommandValidator : AbstractValidator<KeycloakClientResetPasswordCommand>
{
    public KeycloakClientResetPasswordCommandValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);

        RuleFor(r => r.Password)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
    }
}