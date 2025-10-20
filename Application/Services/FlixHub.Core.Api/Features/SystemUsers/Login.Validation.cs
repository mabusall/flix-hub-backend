namespace FlixHub.Core.Api.Features.SystemUsers;

public class LoginSystemUserCommandValidator : AbstractValidator<LoginSystemUserCommand>
{
    public LoginSystemUserCommandValidator(IServiceProvider serviceProvider,
                                           IManagedCancellationToken appToken)
    {
        var uow = serviceProvider.GetRequiredService<IFlixHubDbUnitOfWork>();

        RuleFor(r => r.Password)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);

        RuleFor(r => r.EmailOrAccount)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);

        RuleFor(command => command)
            .MustAsync(async (cmd, _) =>
            {
                // Look up user by email
                var exists = await uow
                        .SystemUsersRepository
                        .AnyAsync(a => (a.Email == cmd.EmailOrAccount || a.Username == cmd.EmailOrAccount) &&
                                  a.Password == cmd.Password.Decrypt() &&
                                  a.IsActive && a.IsVerified, appToken.Token);

                return exists;
            })
            .WithMessage(ErrorMessageResources.UserManagement_InvalidEmailOrPassword)
            .When(w => w.EmailOrAccount is not null && w.Password is not null);
    }
}