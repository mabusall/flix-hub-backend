namespace FlixHub.Core.Api.Entities;

[Table(nameof(Episode))]
class Episode : AuditableEntity
{
    [Required, ForeignKey(nameof(ContentSeason))]
    public long SeasonId { get; set; }

    [Required]
    public int EpisodeNumber { get; set; } // per season

    [Required, MaxLength(200)]
    public string Title { get; set; } = null!;

    [MaxLength(500)]
    public string? Overview { get; set; }

    public DateTime? AirDate { get; set; }

    public int? Runtime { get; set; }

    [MaxLength(512)]
    public string? StillPath { get; set; }

    [Column(TypeName = "decimal(4,2)")]
    public decimal? VoteAverage { get; set; }

    public int? VoteCount { get; set; }

    public virtual ICollection<EpisodeCast> Casts { get; set; } = [];
    public virtual ICollection<EpisodeCrew> Crews { get; set; } = [];
}
