namespace FlixHub.Core.Api.Model;

public record ContentCrewDto : AuditableDto
{
    public long ContentId { get; set; }
    public long PersonId { get; set; }
    public string? CreditId { get; set; }
    public string? Department { get; set; } // Directing, Writing, Production
    public string? Job { get; set; } // Director, Writer, Producer, Screenplay
    public PersonDto? Person { get; set; }
}
