namespace FlixHub.Core.Api.Entities;

[Table(nameof(ContentImage))]
class ContentImage : AuditableEntity
{
    [Required, ForeignKey(nameof(Content))]
    public long ContentId { get; set; }

    [Required]
    public ImageType Type { get; set; } // Poster, Backdrop, Logo, Still

    [Required, MaxLength(512)]
    public string FilePath { get; set; } = null!; // TMDb path

    [MaxLength(8)]
    public string? Language { get; set; } // iso_639_1

    public int Width { get; set; }
    public int Height { get; set; }
}
