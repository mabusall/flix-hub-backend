namespace Keycloak.Api.Features.Client;

public class KeycloakClientDeleteUserCommandValidator : AbstractValidator<KeycloakClientDeleteUserCommand>
{
    public KeycloakClientDeleteUserCommandValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
    }
}