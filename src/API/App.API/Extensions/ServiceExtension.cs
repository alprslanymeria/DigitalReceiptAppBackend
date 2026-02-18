using App.Application.Contracts.Infrastructure.Receipt;
using App.Application.Contracts.Services;
using App.Integration;
using App.Integration.Receipt;

namespace App.API.Extensions;

/// <summary>
/// EXTENSION FOR REGISTERING APPLICATION SERVICES WITH DEPENDENCY INJECTION.
/// NOTE: SERVICES THAT USE CQRS PATTERN ARE REGISTERED VIA MEDIATR AUTOMATICALLY.
/// THIS FILE ONLY CONTAINS SERVICES THAT STILL USE THE TRADITIONAL SERVICE PATTERN.
/// </summary>
public static class ServiceExtension
{
    public static IServiceCollection AddApplicationServicesExt(this IServiceCollection services)
    {
        // COMMON HANDLER SERVICES
        services.AddScoped<IFileStorageHelper, FileStorageHelper>();

        // RECEIPT PROCESSING STRATEGIES (STRATEGY PATTERN)
        services.AddScoped<IReceiptProcessingStrategy, OcrReceiptProcessingStrategy>();
        services.AddScoped<IReceiptProcessingStrategy, QrReceiptProcessingStrategy>();
        services.AddScoped<IReceiptStrategyResolver, ReceiptStrategyResolver>();

        return services;
    }
}
