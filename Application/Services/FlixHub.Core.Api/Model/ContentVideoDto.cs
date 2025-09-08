namespace FlixHub.Core.Api.Model;

public record ContentVideoDto : AuditableDto
{
    public long ContentId { get; set; }
    public VideoType Type { get; set; } // Trailer, Teaser, Clip, Featurette
    public VideoSite Site { get; set; } // YouTube, Vimeo
    public string? Key { get; set; }  // e.g., YouTube video id
    public string? Name { get; set; } // Title of the video
    public bool IsOfficial { get; set; }
}