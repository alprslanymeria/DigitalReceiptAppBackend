using App.Application.Contracts.Infrastructure.AI;
using App.Application.Contracts.Infrastructure.Ocr;
using App.Application.Contracts.Infrastructure.QrCode;
using App.Domain.Options;
using App.Integration.AI;
using App.Integration.QrCode;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Integration;

/// <summary>
/// EXTENSION FOR REGISTERING RECEIPT-RELATED SERVICES (OCR, QR, AI) WITH DI.
/// </summary>
public static class ReceiptServicesExtension
{
    public static IServiceCollection AddReceiptServicesExt(this IServiceCollection services, IConfiguration configuration)
    {
        // OCR SERVICE
        //services.Configure<OcrConfig>(configuration.GetSection(OcrConfig.Key));

        //var ocrConfig = configuration.GetSection(OcrConfig.Key).Get<OcrConfig>() ?? new OcrConfig();

        //services.AddHttpClient<IOcrService, OcrService>(client =>
        //{
        //    if (!string.IsNullOrWhiteSpace(ocrConfig.Endpoint))
        //    {
        //        client.BaseAddress = new Uri(ocrConfig.Endpoint);
        //    }
        //    client.Timeout = TimeSpan.FromSeconds(ocrConfig.TimeoutSeconds);
        //});

        // QR CODE PARSER
        services.Configure<QrCodeConfig>(configuration.GetSection(QrCodeConfig.Key));
        services.AddSingleton<IQrCodeParser, QrCodeParser>();

        // AI PROVIDERS (STRATEGY PATTERN)
        services.Configure<AIProviderConfig>(configuration.GetSection(AIProviderConfig.Key));
        services.AddSingleton<IAIProviderFactory, AIProviderFactory>();

        return services;
    }
}
