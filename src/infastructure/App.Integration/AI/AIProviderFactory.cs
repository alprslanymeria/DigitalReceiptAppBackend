using App.Application.Contracts.Infrastructure.AI;
using App.Domain.Exceptions;
using App.Domain.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Integration.AI;

/// <summary>
/// FACTORY FOR CREATING AND RESOLVING AI ANALYSIS PROVIDERS (STRATEGY PATTERN).
/// READS CONFIGURATION TO BUILD AVAILABLE PROVIDERS AND RESOLVES BY NAME.
/// </summary>
public class AIProviderFactory : IAIProviderFactory
{
    private readonly Dictionary<string, IAIAnalysisProvider> _providers = new(StringComparer.OrdinalIgnoreCase);

    public AIProviderFactory(

        IHttpClientFactory httpClientFactory,
        IOptions<AIProviderConfig> options,
        ILoggerFactory loggerFactory,
        ILogger<AIProviderFactory> logger)
    {
        var config = options.Value;

        foreach (var (name, settings) in config.Providers)
        {
            if (string.IsNullOrWhiteSpace(settings.ApiKey))
            {
                logger.LogWarning("AIProviderFactory -> SKIPPING PROVIDER '{Provider}' — API KEY NOT CONFIGURED", name);
                continue;
            }

            var httpClient = httpClientFactory.CreateClient($"AIProvider_{name}");
            httpClient.Timeout = TimeSpan.FromSeconds(60);

            IAIAnalysisProvider provider = name.ToUpperInvariant() switch
            {
                "OPENAI" => new OpenAIAnalysisProvider(httpClient, settings, config.SystemPrompt, loggerFactory.CreateLogger<OpenAIAnalysisProvider>()),
                "GEMINI" => new GeminiAnalysisProvider(httpClient, settings, config.SystemPrompt, loggerFactory.CreateLogger<GeminiAnalysisProvider>()),
                _ => throw new BusinessException($"UNKNOWN AI PROVIDER TYPE: {name}")
            };

            _providers[name] = provider;
            logger.LogInformation("AIProviderFactory -> REGISTERED AI PROVIDER: {Provider} ({Model})", name, settings.Model);
        }
    }

    public IReadOnlyList<string> AvailableProviders => _providers.Keys.ToList().AsReadOnly();

    public IAIAnalysisProvider GetProvider(string providerName)
    {
        if (!_providers.TryGetValue(providerName, out var provider))
        {
            throw new BusinessException(
                $"AI PROVIDER '{providerName}' IS NOT AVAILABLE. CONFIGURED PROVIDERS: [{string.Join(", ", _providers.Keys)}]");
        }

        return provider;
    }
}
