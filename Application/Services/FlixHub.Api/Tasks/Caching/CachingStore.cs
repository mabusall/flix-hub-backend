namespace Store.Api.Tasks.Caching;

internal class CachingStore(IStoreUnitOfWork storeSession,
                            IMemoryCacheProvider memoryCacheProvider,
                            IManagedCancellationToken applicationLifetime)
{
    public async Task ExecuteAsync()
    {
        var expiration = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.UtcNow.AddDays(1)
        };

        var orders = await storeSession
            .OrdersRepository
            .AsQueryable(false)
            .Include(i => i.Items)
            .ToListAsync(applicationLifetime.Token);

        await memoryCacheProvider.SetStringAsync(nameof(CachingKeys.Orders),
                                                 orders.Adapt<List<OrderDto>>(),
                                                 expiration,
                                                 applicationLifetime.Token);

    }
}