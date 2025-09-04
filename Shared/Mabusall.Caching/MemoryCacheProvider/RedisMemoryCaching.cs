namespace Mabusall.Caching.MemoryCacheProvider;

public class RedisMemoryCaching : IMemoryCacheProvider
{
    #region [ variables ]

    bool _disposed;
    readonly IConnectionMultiplexer _connection;
    readonly IDatabaseAsync _redisDb;

    #endregion

    public RedisMemoryCaching(IConnectionMultiplexer connection)
    {
        _connection = connection;
        _redisDb = _connection.GetDatabase();
    }

    #region [ methods ]

    public T Get<T>(string key) where T : class
    {
        var cached = _redisDb.StringGetAsync(key).Result;
        return !cached.HasValue ? null : JsonSerializer.Deserialize<T>(cached);
    }

    public async Task<T> GetAsync<T>(string key, CancellationToken token) where T : class
    {
        var cached = await _redisDb.StringGetAsync(key);
        return !cached.HasValue ? null : JsonSerializer.Deserialize<T>(cached);
    }

    public void Set<T>(string key, T value, DistributedCacheEntryOptions options) where T : class
    {
        var data = JsonSerializer.Serialize(value);
        _ = _redisDb.StringSetAsync(key, data, options.AbsoluteExpirationRelativeToNow).Result;
    }

    public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token) where T : class
    {
        var data = JsonSerializer.Serialize(value);
        await _redisDb.StringSetAsync(key, data, options.AbsoluteExpirationRelativeToNow);
    }

    public T GetString<T>(string key) where T : class
    {
        var cached = _redisDb.StringGetAsync(key).Result;
        return !cached.HasValue ? null : typeof(T) == typeof(string) ? (T)Convert.ChangeType(cached, typeof(T)) : JsonSerializer.Deserialize<T>(cached);
    }

    public async Task<T> GetStringAsync<T>(string key, CancellationToken token) where T : class
    {
        var cached = await _redisDb.StringGetAsync(key);
        return !cached.HasValue ? null : typeof(T) == typeof(string) ? (T)Convert.ChangeType(cached, typeof(T)) : JsonSerializer.Deserialize<T>(cached);
    }

    public void SetString<T>(string key, T value, DistributedCacheEntryOptions options) where T : class
    {
        var data = typeof(T) == typeof(string) ? (string)Convert.ChangeType(value, typeof(string)) : JsonSerializer.Serialize(value);
        _ = _redisDb.StringSetAsync(key, data, options.AbsoluteExpirationRelativeToNow).Result;
    }

    public async Task SetStringAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token) where T : class
    {
        var data = typeof(T) == typeof(string) ? (string)Convert.ChangeType(value, typeof(string)) : JsonSerializer.Serialize(value);
        await _redisDb.StringSetAsync(key, data, options.AbsoluteExpirationRelativeToNow);
    }

    public async Task<bool> SetStringOnceAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token) where T : class
    {
        var data = typeof(T) == typeof(string) ? (string)Convert.ChangeType(value, typeof(string)) : JsonSerializer.Serialize(value);
        return await _redisDb.StringSetAsync(key, data, options.AbsoluteExpirationRelativeToNow, when: When.NotExists);
    }

    public void Remove(string key)
        => _ = _redisDb.KeyDeleteAsync(key).Result;

    public async Task RemoveAsync(string key, CancellationToken token)
        => await _redisDb.KeyDeleteAsync(key);

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