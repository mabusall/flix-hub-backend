namespace FlixHub.Core.Api.Features.SystemUsers;

internal class LogoutSystemUserCommandHandler(ISender sender)
    : ICommandHandler<LogoutSystemUserCommand, LogoutSystemUserResult>
{
    public async Task<LogoutSystemUserResult> Handle(LogoutSystemUserCommand command, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new LogoutSystemUserResult(true));
    }
}