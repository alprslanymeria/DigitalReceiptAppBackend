using App.Application.Contracts.Infrastructure.Caching;
using App.Caching;
using App.Caching.CacheKey;
using App.Caching.Locker;
using App.Caching.Redis;

namespace App.API.Extensions;

public static class CachingExtension
{
    public static IServiceCollection AddCaching(this IServiceCollection services)
    {

        // CACHING SERVICES
        services.AddTransient(typeof(ICacheKeyStore<>), typeof(CacheKeyStore<>));
        services.AddSingleton<ICacheKeyFactory, CacheKeyFactory>();
        services.AddSingleton<ICacheKeyManager, CacheKeyManager>();
        services.AddSingleton<ILocker, DistributedCacheLocker>();
        services.AddScoped<IShortTermCacheManager, PerRequestCacheManager>();
        services.AddSingleton<IRedisConnectionWrapper, RedisConnectionWrapper>();
        services.AddScoped<IStaticCacheManager, RedisCacheManager>();
        services.AddSingleton<ICacheKeyService, RedisCacheManager>();

        return services;
    }
}

