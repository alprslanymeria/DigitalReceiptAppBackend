using App.Application.Contracts.Infrastructure.Caching;
using App.Caching.CacheKey;
using App.Caching.Locker;
using App.Caching.Redis;
using App.Domain.Options.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace App.Caching;

public static class CachingExtension
{
    public static IServiceCollection AddCachingServicesExt(this IServiceCollection services, IConfiguration configuration)

    {
        var config = configuration.GetRequiredSection("DistributedCacheConfig").Get<RedisCacheConfig>();

        // REDIS CONNECTION MULTIPLEXER
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configurationOptions = ConfigurationOptions.Parse(config.ConnectionString);
            return ConnectionMultiplexer.Connect(configurationOptions);
        });

        // CACHING SERVICES
        services.AddTransient(typeof(ICacheKeyStore<>), typeof(CacheKeyStore<>));
        services.AddSingleton<ICacheKeyFactory, CacheKeyFactory>();
        services.AddSingleton<ICacheKeyManager, CacheKeyManager>();
        services.AddSingleton<ILocker, DistributedCacheLocker>();
        services.AddScoped<IShortTermCacheManager, PerRequestCacheManager>();
        services.AddSingleton<IRedisConnectionWrapper, RedisConnectionWrapper>();
        services.AddScoped<IStaticCacheManager, RedisCacheManager>();
        services.AddSingleton<ICacheKeyService, RedisCacheManager>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config.ConnectionString;
        });

        return services;
    }
}
