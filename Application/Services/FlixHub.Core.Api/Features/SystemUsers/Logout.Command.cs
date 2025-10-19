namespace FlixHub.Core.Api.Features.SystemUsers;

public record LogoutSystemUserCommand
(
    string RefreshToken
) : ICommand<LogoutSystemUserResult>;

public record LogoutSystemUserResult
(
    bool IsSuccess
);