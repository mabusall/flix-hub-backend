namespace Store.Api.Repository;

#region [ interfaces ]

interface IOrdersRepository : IGenericRepository<Order, OrderDto> { }

interface IOrderItemsRepository : IGenericRepository<OrderItem, OrderItemDto> { }

#endregion

#region [ implementation ]

class OrdersRepository(StoreDbContext context)
    : GenericRepository<Order, OrderDto>(context), IOrdersRepository
{ }

class OrderItemsRepository(StoreDbContext context)
    : GenericRepository<OrderItem, OrderItemDto>(context), IOrderItemsRepository
{ }

#endregion