namespace Store.Api.Features.Caching;

internal class GetListCacheQueryHandler
    (IMemoryCacheProvider memoryCacheProvider)
    : IQueryHandler<GetListCacheQuery, string>
{
    public async Task<string> Handle(GetListCacheQuery query, CancellationToken cancellationToken)
    {
        return await memoryCacheProvider
            .GetStringAsync<string>(query.Key.ToString(), cancellationToken);
    }
}