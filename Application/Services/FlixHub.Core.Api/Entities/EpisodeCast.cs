namespace FlixHub.Core.Api.Entities;

[Table(nameof(EpisodeCast))]
class EpisodeCast : AuditableEntity
{
    [Required, ForeignKey(nameof(Episode))]
    public long EpisodeId { get; set; }

    [Required, ForeignKey(nameof(Person))]
    public long PersonId { get; set; }

    [MaxLength(200)]
    public string? Character { get; set; }

    public int? Order { get; set; } // billing order

    public virtual Person? Person { get; set; }
}
