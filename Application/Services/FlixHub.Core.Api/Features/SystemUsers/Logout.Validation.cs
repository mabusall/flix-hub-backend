namespace FlixHub.Core.Api.Features.SystemUsers;

public class LogoutSystemUserCommandValidator : AbstractValidator<LogoutSystemUserCommand>
{
    public LogoutSystemUserCommandValidator()
    {
        RuleFor(r => r.RefreshToken)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);
    }
}