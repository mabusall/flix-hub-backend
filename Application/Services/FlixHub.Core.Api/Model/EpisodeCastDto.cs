namespace FlixHub.Core.Api.Model;

public record EpisodeCastDto : AuditableDto
{
    public long EpisodeId { get; set; }
    public long PersonId { get; set; }
    public string? Character { get; set; }
    public int? Order { get; set; } // billing order
    public PersonDto? Person { get; set; }
}
