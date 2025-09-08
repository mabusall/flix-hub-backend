namespace FlixHub.Core.Api.Entities;

[Table(nameof(ContentRating))]
class ContentRating : AuditableEntity
{
    [Required, ForeignKey(nameof(Content))]
    public long ContentId { get; set; }

    [Required]
    public RatingSource Source { get; set; } // IMDb, Rotten Tomatoes, Metacritic

    [Required, MaxLength(50)]
    public string? Value { get; set; }  // Example: "8.3/10", "98%", "88/100"
}
