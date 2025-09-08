namespace FlixHub.Core.Api.Model;

public record ContentRatingDto : AuditableDto
{
    public long ContentId { get; set; }
    public RatingSource Source { get; set; } // IMDb, Rotten Tomatoes, Metacritic
    public string? Value { get; set; }  // Example: "8.3/10", "98%", "88/100"
}