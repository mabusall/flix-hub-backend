namespace FlixHub.Core.Api.Model;

public record GenreDto : AuditableDto
{
    public int TmdbId { get; set; }
    public string? Name { get; set; }
}
