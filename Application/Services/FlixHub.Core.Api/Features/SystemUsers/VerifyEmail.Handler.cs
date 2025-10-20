namespace FlixHub.Core.Api.Features.SystemUsers;

internal class VerifyEmailSystemUserCommandHandler(IFlixHubDbUnitOfWork uow)
    : ICommandHandler<VerifyEmailSystemUserCommand, VerifyEmailSystemUserResult>
{
    public async Task<VerifyEmailSystemUserResult> Handle(VerifyEmailSystemUserCommand command, CancellationToken cancellationToken)
    {
        var systemUser = await uow
            .SystemUsersRepository
            .AsQueryable(false)
            .FirstOrDefaultAsync(user => user.Email == command.Email &&
                                 user.EmailVerificationCode == command.VerificationCode, cancellationToken);

        // change the flags
        systemUser!.IsActive = systemUser.IsVerified = true;
        systemUser!.EmailVerificationCode = null;

        // update the system user
        uow.SystemUsersRepository.Update(systemUser);

        // commit db changes
        await uow.SaveChangesAsync(cancellationToken);

        // return result
        return new VerifyEmailSystemUserResult(true);
    }
}