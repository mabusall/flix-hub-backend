namespace FlixHub.Core.Api.Features.SystemUsers;

public record LogoutSystemUserCommand
(
    string EmailOrAccount
) : ICommand<LogoutSystemUserResult>;

public record LogoutSystemUserResult
(
    bool IsSuccess
);