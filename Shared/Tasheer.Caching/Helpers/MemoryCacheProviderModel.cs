namespace Tasheer.Caching.Helpers;

public enum CacheProviderType
{
    Memory = 0,
    DistributedMemory,
    RedisMemory,
}

public sealed class MemoryCacheProviderOptions
{
    public string RedisUri { get; set; }
    public string Password { get; set; }
    public string Environment { get; set; }
    public bool IsEnabled { get; set; }
    public CacheProviderType CacheProviderType { get; set; } = CacheProviderType.Memory;
}