namespace FlixHub.Core.Api.Entities;

[Table(nameof(ContentGenre))]
class ContentGenre : AuditableEntity
{
    [ForeignKey(nameof(Content))]
    public long ContentId { get; set; }

    [ForeignKey(nameof(Genre))]
    public long GenreId { get; set; }

    // Navigation
    public virtual Genre? Genre { get; set; }
}