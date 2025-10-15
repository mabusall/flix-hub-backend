namespace FlixHub.Core.Api.Entities;

[Table(nameof(ContentCrew))]
class ContentCrew : AuditableEntity
{
    [ForeignKey(nameof(Content))]
    public long ContentId { get; set; }

    [ForeignKey(nameof(Person))]
    public long PersonId { get; set; }

    [MaxLength(50)]
    public string? CreditId { get; set; }

    [MaxLength(100)]
    public string? Department { get; set; } // Directing, Writing, Production

    [MaxLength(100)]
    public string? Job { get; set; } // Director, Writer, Producer, Screenplay

    // Navigation
    public virtual Person? Person { get; set; }
}