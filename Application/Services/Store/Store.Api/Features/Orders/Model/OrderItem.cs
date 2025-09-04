namespace Store.Api.Features.Orders.Model;

public record OrderItemDto : AuditDto
{
    public long OrderId { get; set; }

    public string? Name { get; set; }
}