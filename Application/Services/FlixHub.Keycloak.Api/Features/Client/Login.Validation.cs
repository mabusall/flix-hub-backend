namespace FlixHub.Keycloak.Api.Features.Client;

public class KeycloakClientLoginCommandValidator : AbstractValidator<KeycloakClientLoginCommand>
{
    public KeycloakClientLoginCommandValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);

        RuleFor(r => r.Password)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
    }
}