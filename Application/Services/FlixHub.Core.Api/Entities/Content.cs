namespace FlixHub.Core.Api.Entities;

[Table(nameof(Content))]
class Content : AuditableEntity
{
    [Required]
    public int TmdbId { get; set; }

    [MaxLength(20)]
    public string? ImdbId { get; set; }

    [Required, Column(TypeName = "varchar(10)")]
    public ContentType Type { get; set; }

    [Required]
    public string? Title { get; set; }

    [MaxLength(200)]
    public string? OriginalTitle { get; set; }

    [MaxLength(500)]
    public string? Overview { get; set; }

    [MaxLength(500)]
    public string? Awards { get; set; }

    [MaxLength(10)]
    public string? OriginalLanguage { get; set; }

    public DateTime? ReleaseDate { get; set; }

    [Column(TypeName = "varchar(50)")]
    public ContentStatus? Status { get; set; }

    [MaxLength(10)]
    public string? Country { get; set; }

    public int? Runtime { get; set; }

    [Column(TypeName = "decimal(12,6)")]
    public decimal? Popularity { get; set; }

    [Column(TypeName = "decimal(4,2)")]
    public decimal? VoteAverage { get; set; }

    public int? VoteCount { get; set; }

    public long? Budget { get; set; }

    [MaxLength(500)]
    public string? PosterPath { get; set; }

    [MaxLength(500)]
    public string? BackdropPath { get; set; }

    [MaxLength(500)]
    public string? LogoPath { get; set; }

    public virtual ICollection<ContentGenre> Genres { get; set; } = [];
    public virtual ICollection<ContentCast> Casts { get; set; } = [];
    public virtual ICollection<ContentCrew> Crews { get; set; } = [];
    public virtual ICollection<ContentRating> Ratings { get; set; } = [];
    public virtual ICollection<ContentImage> Images { get; set; } = [];
    public virtual ICollection<ContentVideo> Videos { get; set; } = [];
    public virtual ICollection<ContentSeason> Seasons { get; set; } = [];
}