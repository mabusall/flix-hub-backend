namespace Store.Contract.OrderItem;

public record OrderItemDto : AuditableEntityDto
{
    public int OrderId { get; set; }

    public string? Name { get; set; }
}
