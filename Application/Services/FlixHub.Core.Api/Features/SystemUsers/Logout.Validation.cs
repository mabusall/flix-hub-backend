namespace FlixHub.Core.Api.Features.SystemUsers;

public class LogoutSystemUserCommandValidator : AbstractValidator<LogoutSystemUserCommand>
{
    public LogoutSystemUserCommandValidator(IServiceProvider serviceProvider,
                                            IManagedCancellationToken appToken)
    {
        var uow = serviceProvider.GetRequiredService<IFlixHubDbUnitOfWork>();

        RuleFor(r => r.EmailOrAccount)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty)
            .DependentRules(() =>
            {
                // check the email not used in the database
                RuleFor(r => r.EmailOrAccount)
                    .MustAsync(async (emailOrAccount, _) =>
                    {
                        var exists = await uow
                            .SystemUsersRepository
                            .AsQueryable(false)
                            .AnyAsync(u => u.Email == emailOrAccount || u.Username == emailOrAccount, appToken.Token);
                        return exists;
                    })
                    .WithMessage(ErrorMessageResources.UserManagement_EmailNotRegistered);
            });
    }
}