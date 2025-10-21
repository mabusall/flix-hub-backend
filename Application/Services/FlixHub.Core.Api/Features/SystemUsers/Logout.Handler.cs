namespace FlixHub.Core.Api.Features.SystemUsers;

internal class LogoutSystemUserCommandHandler(IFlixHubDbUnitOfWork uow,
                                              IMemoryCacheProvider cacheProvider)
    : ICommandHandler<LogoutSystemUserCommand, LogoutSystemUserResult>
{
    public async Task<LogoutSystemUserResult> Handle(LogoutSystemUserCommand command, CancellationToken cancellationToken)
    {
        var user = await uow
            .SystemUsersRepository
            .AsQueryable(false)
            .FirstOrDefaultAsync(user => user.Email == command.EmailOrAccount || user.Username == command.EmailOrAccount, cancellationToken);

        await cacheProvider.RemoveAsync(user!.Email, cancellationToken);

        return await Task.FromResult(new LogoutSystemUserResult(true));
    }
}