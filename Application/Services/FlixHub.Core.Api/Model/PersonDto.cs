namespace FlixHub.Core.Api.Model;

public record PersonDto : AuditableDto
{
    public string? Name { get; set; }
    public GenderType Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public string? KnownForDepartment { get; set; } // Acting, Directing, Writing
    public string? Biography { get; set; }
    public string? ProfilePath { get; set; } // TMDb image path
    public IList<PersonImageDto> Images { get; set; } = [];
}
