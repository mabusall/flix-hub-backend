namespace FlixHub.Core.Api.Services;

// Keep index in cache so it survives service lifetimes (if you keep the same container)
internal sealed class CachedTokenRing(IMemoryCacheProvider cache,
                                      string cacheKey,
                                      IList<string> tokens) : ITokenRing
{
    private readonly object _lock = new();

    private int Index
    {
        get
        {
            // int is a value type, but the cache interface requires reference types.
            // Store as string and parse, or use a boxed int (int?).
            var cached = cache.GetAsync<string>(cacheKey, CancellationToken.None).Result;
            if (cached is not null && int.TryParse(cached, out var idx))
                return idx;
            return 0;
        }
        set
        {
            // Store as string to satisfy the reference type constraint.
            cache.SetAsync(cacheKey,
                           value.ToString(),
                           new DistributedCacheEntryOptions(),
                           CancellationToken.None);
        }
    }

    public string Current
    {
        get
        {
            var idx = Index % tokens.Count;
            return tokens[idx];
        }
    }

    public string PeekNext()
    {
        var idx = (Index + 1) % tokens.Count;
        return tokens[idx];
    }

    public void Advance()
    {
        lock (_lock)
        {
            var next = (Index + 1) % tokens.Count;
            Index = next;
        }
    }

    public void SetIndex(int index)
    {
        lock (_lock)
        {
            Index = ((index % tokens.Count) + tokens.Count) % tokens.Count;
        }
    }
}
