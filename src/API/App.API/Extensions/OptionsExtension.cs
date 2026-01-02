using App.Domain.Options.Caching;
using App.Domain.Options.ExternalAPI;
using App.Domain.Options.Storage;

namespace App.API.Extensions;

public static class OptionsExtension
{
    public static IServiceCollection AddOptionsPatternExt(this IServiceCollection services, IConfiguration configuration)
    {
        // OPTIONS PATTERN
        services.Configure<DistributedCacheConfig>(configuration.GetSection(DistributedCacheConfig.Key));
        services.Configure<CacheConfig>(configuration.GetSection(CacheConfig.Key));
        services.Configure<StorageConfig>(configuration.GetSection(StorageConfig.Key));
        services.Configure<LocalStorageConfig>(configuration.GetSection(LocalStorageConfig.Key));
        services.Configure<GoogleCloudStorageConfig>(configuration.GetSection(GoogleCloudStorageConfig.Key));
        services.Configure<AwsS3StorageConfig>(configuration.GetSection(AwsS3StorageConfig.Key));
        services.Configure<UserApiOptions>(configuration.GetSection(UserApiOptions.Key));

        return services;
    }
}
