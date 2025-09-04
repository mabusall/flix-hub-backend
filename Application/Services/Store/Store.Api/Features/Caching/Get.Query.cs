namespace Store.Api.Features.Caching;

public class GetListCacheQuery : IQuery<string>
{
    public CachingKeys Key { get; set; }
}