namespace FlixHub.Core.Database;

public abstract class AuditableEntity
{
    [Key]
    public int Id { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Uuid { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [MaxLength(50), Column(TypeName = "varchar(50)")]
    public string CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    [MaxLength(50), Column(TypeName = "varchar(50)")]
    public string LastModifiedBy { get; set; }
}