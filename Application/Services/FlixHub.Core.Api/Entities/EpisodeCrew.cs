namespace FlixHub.Core.Api.Entities;

[Table(nameof(EpisodeCrew))]
class EpisodeCrew : AuditableEntity
{
    [Required, ForeignKey(nameof(Episode))]
    public long EpisodeId { get; set; }

    [Required, ForeignKey(nameof(Person))]
    public long PersonId { get; set; }

    [MaxLength(50)]
    public string? CreditId { get; set; }

    [MaxLength(100)]
    public string? Department { get; set; }

    [MaxLength(100)]
    public string? Job { get; set; }

    public virtual Person? Person { get; set; }
}
