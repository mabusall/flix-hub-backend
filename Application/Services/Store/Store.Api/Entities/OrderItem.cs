namespace Store.Api.Entities;

[Table(nameof(OrderItem))]
class OrderItem : AuditableEntity
{
    [Required, ForeignKey(nameof(Order))]
    public long OrderId { get; set; }

    [Required, MaxLength(50), Column(TypeName = "nvarchar")]
    public string? Name { get; set; }

    //public virtual Order? Order { get; set; }
}