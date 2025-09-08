namespace FlixHub.Core.Api.Model;

public record EpisodeCrewDto : AuditableDto
{
    public long EpisodeId { get; set; }
    public long PersonId { get; set; }
    public string? Department { get; set; }
    public string? Job { get; set; }
    public virtual PersonDto? Person { get; set; }
}
