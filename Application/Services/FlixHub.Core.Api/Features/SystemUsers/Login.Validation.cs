namespace FlixHub.Core.Api.Features.SystemUsers;

public class LoginSystemUserCommandValidator : AbstractValidator<LoginSystemUserCommand>
{
    public LoginSystemUserCommandValidator(IServiceProvider serviceProvider,
                                           ISender sender,
                                           IManagedCancellationToken applicationLifetime)
    {
        var sppContext = serviceProvider.GetRequiredService<IFlixHubDbUnitOfWork>();

        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage(ErrorMessageResources.NotEmpty)

            // check if the sign in is mobile user
            .DependentRules(() =>
            {
            });
    }
}