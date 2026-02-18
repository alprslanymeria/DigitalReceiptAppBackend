using App.Domain.Enums;

namespace App.Application.Features.AIAnalysis.Dtos;

/// <summary>
/// RESPONSE DTO FOR AI ANALYSIS RESULT
/// </summary>
public record AIAnalysisResponseDto(
    
    string Id,
    string ReceiptId,
    AnalysisType Type,
    AnalysisStatus Status,
    string ModelProvider,
    string ModelName,
    string? OutputJson,
    string? ErrorMessage,
    int? TokenUsage,
    decimal? CostUsd,
    DateTime CreatedAt

    );