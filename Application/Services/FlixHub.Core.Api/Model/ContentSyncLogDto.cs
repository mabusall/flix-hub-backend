namespace FlixHub.Core.Api.Model;

public record ContentSyncLogDto : AuditableDto
{
    [Required]
    public ContentType Type { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public int Month { get; set; }

    [Required]
    public bool IsCompleted { get; set; }

    [Required]
    public int LastCompletedPage { get; set; }

    public int? TotalPages { get; set; }

    [MaxLength(500), Column(TypeName = "varchar(500)")]
    public string? Notes { get; set; }
}
