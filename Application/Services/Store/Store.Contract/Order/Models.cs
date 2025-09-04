namespace Store.Contract.Order;

public record OrderDto : AuditableEntityDto
{
    public decimal Price { get; set; }

    public decimal Vat { get; set; }

    public decimal TotalPrice { get; set; }

    public OrderType? OrderType { get; set; }

    public int? ParentOrderId { get; set; }

    public IEnumerable<OrderItemDto> Items { get; set; } = [];
}
