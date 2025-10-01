namespace FlixHub.Core.Api.Entities;

[Table(nameof(Genre))]
class Genre : AuditableEntity
{
    [Required]
    public int TmdbReferenceId { get; set; }

    [Required, MaxLength(50)]
    public string? Name { get; set; }
}