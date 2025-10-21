namespace FlixHub.Core.Api.Features.SystemUsers;

public record LoginSystemUserCommand(
    string EmailOrAccount,
    string Password
) : ICommand<LoginSystemUserResult>;
public record LoginSystemUserResult(string Token);
