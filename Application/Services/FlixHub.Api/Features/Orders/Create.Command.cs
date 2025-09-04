namespace Store.Api.Features.Orders;

public record CreateOrderCommand
(
    decimal Price,
    decimal Vat,
    decimal TotalPrice,
    OrderType OrderType,
    IEnumerable<OrderItemDto> Items
) : ICommand<CreateOrderResult>;

internal record CreateOrderResult(Guid Id);