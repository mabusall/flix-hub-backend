namespace Tasheer.Caching.MemoryCacheProvider;

public class DistributedMemoryCaching(IDistributedCache cache) : IMemoryCacheProvider
{
    #region [ variables ]

    bool _disposed;

    #endregion

    #region [ methods ]

    public async Task<T> GetAsync<T>(string key, CancellationToken token)
        where T : class
    {
        var cached = await cache.GetAsync(key, token);
        return cached == null ? null : Serializer.Deserialize<T>(cached);
    }

    public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token)
        where T : class
    {
        var data = Serializer.Serialize(value);
        await cache.SetAsync(key, data, options, token);
    }

    public async Task<T> GetStringAsync<T>(string key, CancellationToken token)
        where T : class
    {
        var cached = await cache.GetStringAsync(key, token);
        return cached == null ? null : typeof(T) == typeof(string) ? (T)Convert.ChangeType(cached, typeof(T)) : JsonSerializer.Deserialize<T>(cached);
    }

    public async Task SetStringAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token)
        where T : class
    {
        var json = JsonSerializer.Serialize(value);
        await cache.SetStringAsync(key, json, options, token);
    }

    public async Task<bool> SetStringOnceAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token)
        where T : class
    {
        var cached = await GetStringAsync<T>(key, token);
        if (cached != null) return false;

        var json = JsonSerializer.Serialize(value);
        await cache.SetStringAsync(key, json, options, token);
        return true;
    }

    public async Task RemoveAsync(string key, CancellationToken token)
        => await cache.RemoveAsync(key, token);

    #region [ dispose implementation ]

    // Protected implementation of Dispose pattern.
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // dispose managed state (managed objects)
            }

            // free unmanaged resources (unmanaged objects) and override finalizer
            // set large fields to null
            _disposed = true;
        }
    }

    // Public implementation of Dispose pattern callable by consumers.
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion

    #endregion
}