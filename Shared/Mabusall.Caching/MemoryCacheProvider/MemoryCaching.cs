namespace Mabusall.Caching.MemoryCacheProvider;

public class MemoryCaching(IMemoryCache cache) : IMemoryCacheProvider
{
    #region [ variables ]

    bool _disposed;

    #endregion

    #region [ methods ]

    public T Get<T>(string key) where T : class
    {
        var cached = cache.Get(key) as byte[];
        return cached == null ? null : Serializer.Deserialize<T>(cached);
    }

    public Task<T> GetAsync<T>(string key, CancellationToken token) where T : class
        => Task.Run(() => Get<T>(key), token);

    public void Set<T>(string key, T value, DistributedCacheEntryOptions options) where T : class
    {
        var data = Serializer.Serialize(value);
        cache.Set(key, data, options.AbsoluteExpiration ?? DateTimeOffset.Now.AddDays(1));
    }

    public Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token) where T : class
        => Task.Run(() => Set(key, value, options), token);

    public T GetString<T>(string key) where T : class
    {
        var cached = cache.Get<string>(key);
        return cached == null ? null : typeof(T) == typeof(string) ? (T)Convert.ChangeType(cached, typeof(T)) : JsonSerializer.Deserialize<T>(cached);
    }

    public Task<T> GetStringAsync<T>(string key, CancellationToken token) where T : class
        => Task.Run(() => Get<T>(key), token);

    public void SetString<T>(string key, T value, DistributedCacheEntryOptions options) where T : class
    {
        var json = JsonSerializer.Serialize(value);
        cache.Set(key, json, options.AbsoluteExpiration ?? DateTimeOffset.Now.AddDays(1));
    }

    public Task SetStringAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token) where T : class
        => Task.Run(() => Set(key, value, options), token);

    public async Task<bool> SetStringOnceAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token) where T : class
    {
        var data = Serializer.Serialize(value);
        var result = await Task.Run(() => cache.Set(key, data, options.AbsoluteExpiration ?? DateTimeOffset.Now.AddDays(1)), token);
        return result != null;
    }

    public void Remove(string key)
        => cache.Remove(key);

    public Task RemoveAsync(string key, CancellationToken token)
        => Task.Run(() => Remove(key), token);

    #region [ dispose implementation ]

    // Protected implementation of Dispose pattern.
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // dispose managed state (managed objects)
                cache.Dispose();
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