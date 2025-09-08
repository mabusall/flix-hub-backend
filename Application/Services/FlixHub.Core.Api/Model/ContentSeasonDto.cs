namespace FlixHub.Core.Api.Model;

public record ContentSeasonDto : AuditableDto
{
    public long ContentId { get; set; }
    public int SeasonNumber { get; set; } // 1, 2, 3...
    public string? Title { get; set; } // sometimes named differently
    public string? Overview { get; set; }
    public DateTime? AirDate { get; set; }
    public int? EpisodeCount { get; set; }
    public string? PosterPath { get; set; } // TMDb image path
    public virtual IList<EpisodeDto> Episodes { get; set; } = [];
}
