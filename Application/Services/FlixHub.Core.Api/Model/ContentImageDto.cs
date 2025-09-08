namespace FlixHub.Core.Api.Model;

public record ContentImageDto : AuditableDto
{
    public long ContentId { get; set; }
    public ImageType Type { get; set; } // Poster, Backdrop, Logo, Still
    public string FilePath { get; set; } = null!; // TMDb path
    public string? Language { get; set; } // iso_639_1
    public int Width { get; set; }
    public int Height { get; set; }
}
