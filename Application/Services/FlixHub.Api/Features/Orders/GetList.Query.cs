namespace Store.Api.Features.Orders;

public class GetListOrderQuery : PagingBase,
  IQuery<PaginatedList<OrderDto>>
{
    public int? Id { get; set; }
    public OrderType? OrderTypeId { get; set; }
    public bool? IncludeOrderItems { get; set; }
}