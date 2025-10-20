namespace FlixHub.Core.Api.Features.SystemUsers;

public class CreateSystemUserCommandValidator : AbstractValidator<CreateSystemUserCommand>
{
    public CreateSystemUserCommandValidator(IServiceProvider serviceProvider,
                                            IManagedCancellationToken appToken)
    {
        var sppContext = serviceProvider.GetRequiredService<IFlixHubDbUnitOfWork>();

        RuleFor(r => r.FirstName)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);

        RuleFor(r => r.LastName)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty);

        RuleFor(r => r.Password)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty)
            // at least 8 characters, one uppercase, one lowercase, one digit, one special character
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
            .WithMessage(ErrorMessageResources.UserManagement_WeakPassword);

        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty)
            .Matches("^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$")
            .WithMessage(ErrorMessageResources.UserManagement_InvalidEmailAddress)
            .DependentRules(() =>
            {
                // check the email not used in the database
                RuleFor(r => r.Email)
                    .MustAsync(async (email, _) =>
                    {
                        var exists = await sppContext
                            .SystemUsersRepository
                            .AsQueryable(false)
                            .AnyAsync(u => u.Email == email, appToken.Token);
                        return !exists;
                    })
                    .WithMessage(ErrorMessageResources.UserManagement_EmailAlreadyExists);
            });

        RuleFor(r => r.Username)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty)
            .DependentRules(() =>
            {
                // check the username not used in the database
                RuleFor(r => r.Username)
                    .MustAsync(async (username, _) =>
                    {
                        var exists = await sppContext
                        .SystemUsersRepository
                            .AsQueryable(false)
                            .AnyAsync(u => u.Username == username, appToken.Token);
                        return !exists;
                    })
                    .WithMessage(ErrorMessageResources.UserManagement_UsernameAlreadyExists);
            });
    }
}