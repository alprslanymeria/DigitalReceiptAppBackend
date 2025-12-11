using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace App.Infrastructure.OpenTelemetry;

public static class Logging
{
    public static void AddOpenTelemetryLog(this WebApplicationBuilder builder)
    {
        // GET OPENTELEMETRY SETTINGS FROM CONFIGURATION
        var otelSection = builder.Configuration.GetSection("OpenTelemetry");
        var serviceName = otelSection.GetValue<string>("ServiceName");
        var serviceVersion = otelSection.GetValue<string>("ServiceVersion");

        builder.Logging.AddOpenTelemetry(cfg =>
        {
            // FILL RESOURCE BUILDER WITH SERVICE NAME AND VERSION
            var resourceBuilder = ResourceBuilder
                                    .CreateDefault()
                                    .AddService(serviceName ?? "UnknownService", serviceVersion: serviceVersion);

            // SET RESOURCE BUILDER TO CONFIGURATION
            cfg.SetResourceBuilder(resourceBuilder);

            // ADD OTLP EXPORTER TO SEND LOGS TO OTEL COLLECTOR
            cfg.AddOtlpExporter();
        });
    }
}