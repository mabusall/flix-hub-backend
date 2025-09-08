namespace FlixHub.Core.Api.Entities;

[Table(nameof(ContentVideo))]
class ContentVideo : AuditableEntity
{
    [Required, ForeignKey(nameof(Content))]
    public long ContentId { get; set; }

    [Required]
    public VideoType Type { get; set; } // Trailer, Teaser, Clip, Featurette

    [Required]
    public VideoSite Site { get; set; } // YouTube, Vimeo

    [Required, MaxLength(64)]
    public string? Key { get; set; }  // e.g., YouTube video id

    [Required, MaxLength(256)]
    public string? Name { get; set; } // Title of the video

    [Required]
    public bool IsOfficial { get; set; }
}
