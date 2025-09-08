namespace FlixHub.Core.Api.Entities;

[Table(nameof(Person))]
class Person : AuditableEntity
{
    [Required, MaxLength(150)]
    public string? Name { get; set; }

    [Required]
    public GenderType Gender { get; set; }

    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }

    [MaxLength(100)]
    public string? KnownForDepartment { get; set; } // Acting, Directing, Writing

    public string? Biography { get; set; }

    public string? ProfilePath { get; set; } // TMDb image path

    public virtual ICollection<PersonImage> Images { get; set; } = [];
}