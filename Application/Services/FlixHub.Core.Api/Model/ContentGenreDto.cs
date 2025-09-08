namespace FlixHub.Core.Api.Model;

public record ContentGenreDto : AuditableDto
{
    public long ContentId { get; set; }
    public long GenreId { get; set; }
    public GenreDto? Genre { get; set; }
}
