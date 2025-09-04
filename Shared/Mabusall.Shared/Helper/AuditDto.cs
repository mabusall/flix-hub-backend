namespace Tasheer.Shared.Helper;

public record AuditDto
{
    public Guid Uuid { get; set; }

    public DateTime Created { get; set; }

    [JsonIgnore]
    public string CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    [JsonIgnore]
    public string LastModifiedBy { get; set; }
}
