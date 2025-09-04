namespace Store.Api.Features.Orders.Model;

public record OrderDto : AuditDto
{
    public decimal Price { get; set; }

    public decimal Vat { get; set; }

    public decimal TotalPrice { get; set; }

    public OrderType? OrderType { get; set; }

    public List<OrderItemDto> Items { get; set; } = [];
}