namespace FlixHub.Core.Api.Features.SystemUsers;

public class VerifyEmailSystemUserCommandValidator : AbstractValidator<VerifyEmailSystemUserCommand>
{
    public VerifyEmailSystemUserCommandValidator(IServiceProvider serviceProvider,
                                                 IManagedCancellationToken appToken)
    {
        var uow = serviceProvider.GetRequiredService<IFlixHubDbUnitOfWork>();

        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);

        RuleFor(r => r.VerificationCode)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);

        RuleFor(command => command)
            .MustAsync(async (cmd, _) =>
            {
                var exists = await uow
                            .SystemUsersRepository
                            .AsQueryable(false)
                            .AnyAsync(a => a.EmailVerificationCode == cmd.VerificationCode && a.Email == cmd.Email, appToken.Token);
                return exists;
            })
            .WithMessage(ErrorMessageResources.UserManagement_InvalidVerificationCode)
            .When(w => w.Email is not null && w.VerificationCode is not null);
    }
}