namespace Store.Api.Entities;

[Table(nameof(Order))]
class Order : AuditableEntity
{
    [Required, Column(TypeName = "money")]
    public decimal Price { get; set; }

    [Required, Column(TypeName = "money")]
    public decimal Vat { get; set; }

    [Required, Column(TypeName = "money")]
    public decimal TotalPrice { get; set; }

    public OrderType? OrderType { get; set; }

    public ICollection<OrderItem> Items { get; set; } = [];
}