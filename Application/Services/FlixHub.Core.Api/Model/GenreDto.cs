namespace FlixHub.Core.Api.Model;

public record GenreDto : AuditableDto
{
    public int TmdbReferenceId { get; set; }
    public string? Name { get; set; }
}
