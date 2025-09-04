interface IStoreUnitOfWork : IUnitOfWork
{
    IOrdersRepository OrdersRepository { get; }

    IOrderItemsRepository OrderItemsRepository { get; }
}

class StoreUnitOfWork(StoreDbContext context) : UnitOfWork(context), IStoreUnitOfWork
{
    public IOrdersRepository OrdersRepository => new OrdersRepository(context);

    public IOrderItemsRepository OrderItemsRepository => new OrderItemsRepository(context);
}