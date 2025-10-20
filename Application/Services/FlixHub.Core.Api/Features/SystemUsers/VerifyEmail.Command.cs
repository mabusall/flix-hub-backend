namespace FlixHub.Core.Api.Features.SystemUsers;

public record VerifyEmailSystemUserCommand
(
    string Email,
    string VerificationCode
) : ICommand<VerifyEmailSystemUserResult>;

public record VerifyEmailSystemUserResult(bool IsSuccess);