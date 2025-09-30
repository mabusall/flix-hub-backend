namespace FlixHub.Core.Api.Entities;

[Table(nameof(DailyApiUsage))]
class DailyApiUsage : AuditableEntity
{
    [Required]
    public DateTime Date { get; set; }

    [Required]
    public ContentType ContentType { get; set; }

    [Required]
    public int RequestCount { get; set; }
}
