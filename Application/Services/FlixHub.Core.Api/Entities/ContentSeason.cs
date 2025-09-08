namespace FlixHub.Core.Api.Entities;

[Table(nameof(ContentSeason))]
class ContentSeason : AuditableEntity
{
    [Required, ForeignKey(nameof(Content))]
    public long ContentId { get; set; }

    [Required]
    public int SeasonNumber { get; set; } // 1, 2, 3...

    [MaxLength(200)]
    public string? Title { get; set; } // sometimes named differently

    [MaxLength(500)]
    public string? Overview { get; set; }

    public DateTime? AirDate { get; set; }

    public int? EpisodeCount { get; set; }

    [MaxLength(512)]
    public string? PosterPath { get; set; } // TMDb image path

    public virtual ICollection<Episode> Episodes { get; set; } = [];
}
