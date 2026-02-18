using App.Application.Contracts.Infrastructure.AI;
using App.Domain.Options;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace App.Integration.AI;

/// <summary>
/// OPENAI-BASED AI ANALYSIS PROVIDER
/// CALLS OPENAI CHAT COMPLETIONS API WITH RECEIPT CONTEXT AND USER PROMPT.
/// </summary>
public class OpenAIAnalysisProvider(

    HttpClient httpClient,
    AIProviderSettings settings,
    string systemPrompt,
    ILogger<OpenAIAnalysisProvider> logger) : IAIAnalysisProvider
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public string ProviderName => "OpenAI";

    public async Task<AIAnalysisResult> AnalyzeAsync(string prompt, string receiptDataJson, CancellationToken ct = default)
    {
        logger.LogInformation("OpenAIAnalysisProvider -> ANALYZING WITH MODEL: {Model}", settings.Model);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.ApiKey);

        var requestBody = new
        {
            model = settings.Model,
            messages = new object[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = $"Receipt Data:\n{receiptDataJson}\n\nUser Question:\n{prompt}" }
            },
            max_tokens = settings.MaxTokens,
            temperature = settings.Temperature
        };

        var json = JsonSerializer.Serialize(requestBody, JsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync($"{settings.Endpoint}/chat/completions", content, ct);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(ct);
        var parsed = JsonSerializer.Deserialize<JsonElement>(responseJson, JsonOptions);

        var outputText = parsed.GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? string.Empty;

        int? totalTokens = null;
        if (parsed.TryGetProperty("usage", out var usage) && usage.TryGetProperty("total_tokens", out var tokens))
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
