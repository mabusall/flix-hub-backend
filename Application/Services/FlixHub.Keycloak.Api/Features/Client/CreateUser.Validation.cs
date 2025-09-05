namespace FlixHub.Keycloak.Api.Features.Client;

public class KeycloakClientCreateUserCommandValidator : AbstractValidator<KeycloakClientCreateUserCommand>
{
    public KeycloakClientCreateUserCommandValidator()
    {
        RuleFor(r => r.UserName)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
        RuleFor(r => r.FirstName)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
        RuleFor(r => r.LastName)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
    }
}