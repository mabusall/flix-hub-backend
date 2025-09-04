namespace Tasheer.Caching;

public static class DependencyInjection
{
    public static IServiceCollection AddMemoryCaching(this IServiceCollection services, Action<MemoryCacheProviderOptions> configuration)
    {
        var options = new MemoryCacheProviderOptions();

        // copy all options
        configuration(options);

        switch (options.CacheProviderType)
        {
            case CacheProviderType.Memory:
                services.AddMemoryCache();
                services.AddSingleton<IMemoryCacheProvider, MemoryCaching>();
                break;

            case CacheProviderType.DistributedMemory:
                services.AddDistributedMemoryCache();
                services.AddSingleton<IMemoryCacheProvider, DistributedMemoryCaching>();
                break;

            case CacheProviderType.RedisMemory:

                if (!options.IsEnabled)
                {
                    services.AddDistributedMemoryCache();
                    services.AddSingleton<IMemoryCacheProvider, DistributedMemoryCaching>();
                }
                else
                {
                    var redisOptions = ConfigurationOptions.Parse(options.RedisUri);
                    redisOptions.Password = options.Password;
                    redisOptions.AllowAdmin = false;
                    redisOptions.AbortOnConnectFail = false;
                    redisOptions.Ssl = false;

                    if (options.Environment.Equals("QA", StringComparison.OrdinalIgnoreCase))
                    {
                        services.AddStackExchangeRedisCache(setup =>
                        {
                            setup.ConfigurationOptions = redisOptions;
                        });
                        services.AddSingleton<IMemoryCacheProvider, DistributedMemoryCaching>();
                    }
                    else
                    {
                        //Redis-Dependency Injection Of The ConnectionMultiplexer
                        services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(redisOptions));
                        services.AddSingleton<IMemoryCacheProvider, RedisMemoryCaching>();
                    }
                }
                break;
        }

        return services;
    }
}