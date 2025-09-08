namespace FlixHub.Core.Api.Model;

public record PersonImageDto : AuditableDto
{
    public long PersonId { get; set; }
    public ImageType Type { get; set; } // Profile only, but kept flexible
    public string FilePath { get; set; } = null!; // TMDb path
    public string? Language { get; set; } // iso_639_1
    public int Width { get; set; }
    public int Height { get; set; }
}
