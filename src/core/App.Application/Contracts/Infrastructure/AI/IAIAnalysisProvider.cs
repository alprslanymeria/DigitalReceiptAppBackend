namespace App.Application.Contracts.Infrastructure.AI;


/// <summary>
/// STRATEGY INTERFACE FOR AI ANALYSIS PROVIDERS.
/// EACH IMPLEMENTATION REPRESENTS A DIFFERENT AI PROVIDER
/// </summary>
public interface IAIAnalysisProvider
{
    string ProviderName { get; }
    Task<AIAnalysisResult> AnalyzeAsync(string prompt, string receiptDataJson, CancellationToken ct = default);
}

/// <summary>
/// RESULT OF AI ANALYSIS ON RECEIPT DATA.
/// </summary>
public record AIAnalysisResult
{
    public string OutputJson { get; init; } = null!;
    public int? TokenUsage { get; init; }
    public decimal? CostUsd { get; init; }
    public string ModelName { get; init; } = null!;
    public string? ModelVersion { get; init; }
}