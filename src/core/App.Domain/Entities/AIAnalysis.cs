using App.Domain.Enums;

namespace App.Domain.Entities;

public class AIAnalysis : AuditableEntity<string>
{
    public string ReceiptId { get; set; } = null!;
    public AnalysisType Type { get; set; }
    public AnalysisStatus Status { get; set; }
    public string ModelProvider { get; set; } = null!;
    public string ModelName { get; set; } = null!;
    public string? ModelVersion { get; set; }
    public string? InputJson { get; set; }
    public string? OutputJson { get; set; }
    public string? ErrorMessage { get; set; }
    public int RetryCount { get; set; }
    public int? TokenUsage { get; set; }
    public decimal? CostUsd { get; set; }

    // REFERENCES (PARENTS)
    public Receipt? Receipt { get; set; } // FOR ReceiptId
}
