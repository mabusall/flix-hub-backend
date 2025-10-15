namespace FlixHub.Core.Api.Entities;

[Table(nameof(ContentCast))]
class ContentCast : AuditableEntity
{
    [ForeignKey(nameof(Content))]
    public long ContentId { get; set; }

    [ForeignKey(nameof(Person))]
    public long PersonId { get; set; }

    [MaxLength(50)]
    public string? CreditId { get; set; }

    [MaxLength(200)]
    public string? Character { get; set; } // Name of the character played

    public int? Order { get; set; } // Billing order from TMDb

    // Navigation
    public virtual Person? Person { get; set; }
}
