namespace FlixHub.Core.Api.Features.SystemUsers;

public record LoginSystemUserCommand(
    string Email,
    string Password
) : ICommand<LoginSystemUserResult>;
public record LoginSystemUserResult(Guid UserId);
