namespace FlixHub.Core.Api.Features.SystemUsers;

public record CreateSystemUserCommand : ICommand<CreateSystemUserResult>
{
    // common fields for all users type
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

public record CreateSystemUserResult(bool IsSuccess);