namespace App.Application.Features.AIAnalysis.Dtos;

/// <summary>
/// REQUEST DTO FOR ANALYZING RECEIPTS WITH AI
/// </summary>
public record AnalyzeReceiptRequest(
    
    string ReceiptId,
    string Prompt,
    string? ProviderName
    );