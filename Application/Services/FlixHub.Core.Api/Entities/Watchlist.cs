namespace FlixHub.Core.Api.Entities;

[Table(nameof(Watchlist))]
class Watchlist : AuditableEntity
{
    [Required, ForeignKey(nameof(SystemUser))]
    public long UserId { get; set; }

    [Required, ForeignKey(nameof(Content))]
    public long ContentId { get; set; }

    [Required]
    public DateTime AddedAt { get; set; }

    public virtual Content? Content { get; set; }
}
