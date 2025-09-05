namespace FlixHub.Core.Database;

public abstract class AuditableEntity
{
    [Key]
    public int Id { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Uuid { get; set; }

    [Required, Column(TypeName = "datetime2(7)")]
    public DateTime Created { get; set; }

    [MaxLength(50), Column(TypeName = "varchar(50)")]
    public string CreatedBy { get; set; }

    [Column(TypeName = "datetime2(7)")]
    public DateTime? LastModified { get; set; }

    [MaxLength(50), Column(TypeName = "varchar(50)")]
    public string LastModifiedBy { get; set; }
}