namespace FlixHub.Core.Api.Entities;

[Table(nameof(SystemUser))]
class SystemUser : AuditableEntity
{
    [Required, MaxLength(150), Column(TypeName = "varchar")]
    public string? Username { get; set; }

    [Required, MaxLength(150), Column(TypeName = "varchar")]
    public string? Email { get; set; }

    [Required, MaxLength(150), Column(TypeName = "varchar")]
    public string? Password { get; set; }

    [Required, MaxLength(150), Column(TypeName = "varchar")]
    public string? FirstName { get; set; }

    [Required, MaxLength(150), Column(TypeName = "varchar")]
    public string? LastName { get; set; }

    [MaxLength(200), Column(TypeName = "varchar")]
    public string? EmailVerificationCode { get; set; }

    [Required]
    public bool IsActive { get; set; }

    [Required]
    public bool IsVerified { get; set; }

    // This will be stored as JSONB in PostgreSQL
    [Column(TypeName = "jsonb")]
    public UserPreferencesDto? Preferences { get; set; }

    public virtual ICollection<Watchlist> Watchlist { get; set; } = [];
}