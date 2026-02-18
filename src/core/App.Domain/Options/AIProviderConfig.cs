namespace App.Domain.Options;

/// <summary>
/// CONFIGURATION OPTIONS FOR AI ANALYSIS PROVIDERS (STRATEGY PATTERN).
/// </summary>
public class AIProviderConfig
{
    public const string Key = "AIProviderConfig";

    public string DefaultProvider { get; set; } = "OpenAI";
    public Dictionary<string, AIProviderSettings> Providers { get; set; } = [];
    public string SystemPrompt { get; set; } = "You are a receipt analysis assistant. Only answer questions related to receipt data, spending patterns, and financial insights. If the question is not related to receipts or financial data, politely decline to answer.";
}

/// <summary>
/// SETTINGS FOR A SINGLE AI PROVIDER.
/// </summary>
public class AIProviderSettings
{
    public string ApiKey { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string Endpoint { get; set; } = null!;
    public int MaxTokens { get; set; } = 2000;
    public double Temperature { get; set; } = 0.3;
}
