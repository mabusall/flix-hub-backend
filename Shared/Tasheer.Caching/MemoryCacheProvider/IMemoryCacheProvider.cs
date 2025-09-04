
namespace Tasheer.Caching.MemoryCacheProvider;

public interface IMemoryCacheProvider : IDisposable
{
    Task<T> GetAsync<T>(string key, CancellationToken token)
        where T : class;

    Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token)
        where T : class;

    Task SetStringAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token)
        where T : class;

    Task<T> GetStringAsync<T>(string key, CancellationToken token)
        where T : class;

    Task<bool> SetStringOnceAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token)
        where T : class;

    Task RemoveAsync(string key, CancellationToken token);
}
