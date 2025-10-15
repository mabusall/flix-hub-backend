namespace FlixHub.Core.Api.Model;

public record EpisodeDto : AuditableDto
{
    public long SeasonId { get; set; }
    public int EpisodeNumber { get; set; } // per season
    public string Title { get; set; } = null!;
    public string? Overview { get; set; }
    public DateTime? AirDate { get; set; }
    public int? Runtime { get; set; }
    public string? StillPath { get; set; }
    public decimal? VoteAverage { get; set; }
    public int? VoteCount { get; set; }
    public IList<EpisodeCrewDto> Crews { get; set; } = [];
}
