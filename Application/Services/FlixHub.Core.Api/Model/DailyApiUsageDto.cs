namespace FlixHub.Core.Api.Model;

public record DailyApiUsageDto : AuditableDto
{
    [Required]
    public DateTime Date { get; set; }

    [Required]
    public ContentType ContentType { get; set; }

    [Required]
    public int RequestCount { get; set; }
}