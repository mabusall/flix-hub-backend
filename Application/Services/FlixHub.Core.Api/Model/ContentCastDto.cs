namespace FlixHub.Core.Api.Model;

public record ContentCastDto : AuditableDto
{
    public long ContentId { get; set; }
    public long PersonId { get; set; }
    public string? Character { get; set; } // Name of the character played
    public string? CreditId { get; set; }
    public int? Order { get; set; } // Billing order from TMDb
    public PersonDto? Person { get; set; }
}