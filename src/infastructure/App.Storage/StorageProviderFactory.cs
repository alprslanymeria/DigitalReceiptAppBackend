using App.Application.Contracts.Infrastructure.Storage;
using App.Domain.Options.Storage;
using App.Storage.aws;
using App.Storage.google;
using App.Storage.local;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace App.Storage;

/// <summary>
/// FACTORY FOR CREATING STORAGE PROVIDERS BASED ON CONFIGURATION
/// </summary>
public class StorageProviderFactory(

    IServiceProvider serviceProvider,
    IOptions<StorageConfig> config

) : IStorageProviderFactory

{
    private readonly StorageConfig _config = config.Value;

    public IStorageProvider CreateProvider(StorageType storageType)
    {
        return storageType switch
        {
            StorageType.Local => serviceProvider.GetRequiredService<LocalFileStorageProvider>(),
            StorageType.GoogleCloud => serviceProvider.GetRequiredService<GoogleCloudStorageProvider>(),
            StorageType.AwsS3 => serviceProvider.GetRequiredService<AwsS3StorageProvider>(),
            StorageType.AzureBlob => throw new NotSupportedException("Azure Blob Storage is not yet implemented. Add the implementation and register it."),
            _ => throw new ArgumentOutOfRangeException(nameof(storageType), $"Unknown storage type: {storageType}")
        };
    }

    public IStorageProvider GetDefaultProvider()
    {
        return CreateProvider(_config.StorageType);
    }
}