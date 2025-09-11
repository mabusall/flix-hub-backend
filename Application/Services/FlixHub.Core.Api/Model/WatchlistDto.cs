namespace FlixHub.Core.Api.Model;

public record WatchlistDto : AuditableDto
{
    [Required, ForeignKey(nameof(SystemUser))]
    public long UserId { get; set; }

    [Required, ForeignKey(nameof(Content))]
    public long ContentId { get; set; }

    [Required]
    public DateTime AddedAt { get; set; }

    public ContentDto? Content { get; set; }
}