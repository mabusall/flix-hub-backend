namespace FlixHub.Core.Api.Entities;

[Table(nameof(PersonImage))]
class PersonImage : AuditableEntity
{
    [Required, ForeignKey(nameof(Person))]
    public long PersonId { get; set; }

    [Required]
    public ImageType Type { get; set; } // Profile only, but kept flexible

    [Required, MaxLength(512)]
    public string FilePath { get; set; } = null!; // TMDb path

    [MaxLength(8)]
    public string? Language { get; set; } // iso_639_1

    public int Width { get; set; }
    public int Height { get; set; }
}
