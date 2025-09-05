namespace FlixHub.Core.Api.Model;

public record SystemUserDto : AuditDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public Guid? KeycloakUserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EmailVerificationCode { get; set; }
    public bool IsActive { get; set; }
    public bool IsVerified { get; set; }
    public UserPreferencesDto Preferences { get; set; } = new();
}