using App.Application.Contracts.Infrastructure.AI;
using App.Domain.Options;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace App.Integration.AI;

/// <summary>
/// GOOGLE GEMINI-BASED AI ANALYSIS PROVIDER (STRATEGY).
/// CALLS GEMINI GENERATE CONTENT API WITH RECEIPT CONTEXT AND USER PROMPT.
/// </summary>
public class GeminiAnalysisProvider(

    HttpClient httpClient,
    AIProviderSettings settings,
    string systemPrompt,
    ILogger<GeminiAnalysisProvider> logger) : IAIAnalysisProvider
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public string ProviderName => "Gemini";

    public async Task<AIAnalysisResult> AnalyzeAsync(string prompt, string receiptDataJson, CancellationToken ct = default)
    {
        logger.LogInformation("GeminiAnalysisProvider -> ANALYZING WITH MODEL: {Model}", settings.Model);

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = $"{systemPrompt}\n\nReceipt Data:\n{receiptDataJson}\n\nUser Question:\n{prompt}" }
                    }
                }
            },
            generationConfig = new
            {
                maxOutputTokens = settings.MaxTokens,
                temperature = settings.Temperature
            }
        };

        var json = JsonSerializer.Serialize(requestBody, JsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var url = $"{settings.Endpoint}/models/{settings.Model}:generateContent?key={settings.ApiKey}";
        var response = await httpClient.PostAsync(url, content, ct);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(ct);
        var parsed = JsonSerializer.Deserialize<JsonElement>(responseJson, JsonOptions);

        var outputText = parsed.GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString() ?? string.Empty;

        int? totalTokens = null;
        if (parsed.TryGetProperty("usageMetadata", out var usage) && usage.TryGetProperty("totalTokenCount", out var tokens))
        {
            totalTokens = tokens.GetInt32();
        }

        return new AIAnalysisResult
        {
            OutputJson = outputText,
            ModelName = settings.Model,
            ModelVersion = null,
            TokenUsage = totalTokens,
            CostUsd = null
        };
    }
}
