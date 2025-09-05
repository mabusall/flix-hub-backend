namespace FlixHub.Core.Api.Entities;

[Table(nameof(SystemUser))]
class SystemUser : AuditableEntity
{
    [Required, MaxLength(150), Column(TypeName = "varchar")]
    public string? Username { get; set; }

    [Required, MaxLength(150), Column(TypeName = "varchar")]
    public string? Email { get; set; }

    public Guid? KeycloakUserId { get; set; }

    [Required, MaxLength(150), Column(TypeName = "nvarchar")]
    public string? FirstName { get; set; }

    [Required, MaxLength(150), Column(TypeName = "nvarchar")]
    public string? LastName { get; set; }

    [MaxLength(200), Column(TypeName = "varchar")]
    public string? EmailVerificationCode { get; set; }

    [Required]
    public bool IsActive { get; set; }

    [Required]
    public bool IsVerified { get; set; }
}
