namespace Store.Api.Features.Orders;

internal class GetListOrderQueryHandler
    (IStoreUnitOfWork storeSession)
    : IQueryHandler<GetListOrderQuery, PaginatedList<OrderDto>>
{
    public async Task<PaginatedList<OrderDto>> Handle(GetListOrderQuery query, CancellationToken cancellationToken)
    {
        var queryBuilder = storeSession
            .OrdersRepository
            .AsQueryable(false);

        if (query.IncludeOrderItems ?? false)
            queryBuilder = queryBuilder.Include(i => i.Items);

        if (query.Id is not null)
            queryBuilder = queryBuilder.Where(w => w.Id == query.Id);

        if (query.OrderTypeId is not null)
            queryBuilder = queryBuilder.Where(w => w.OrderType == query.OrderTypeId);

        // execute query on db
        return await storeSession.OrdersRepository.GetPaginatedListAsync(queryBuilder,
                                                                         query.PageNumber,
                                                                         query.PageSize,
                                                                         query.SortBy,
                                                                         cancellationToken);
    }
}