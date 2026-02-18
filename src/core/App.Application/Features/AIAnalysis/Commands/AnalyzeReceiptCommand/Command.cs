using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.AIAnalysis.Dtos;

namespace App.Application.Features.AIAnalysis.Commands.AnalyzeReceiptCommand;

/// <summary>
/// COMMAND TO ANALYZE A RECEIPT USING AI.
/// USES STRATEGY PATTERN VIA IAIProviderFactory TO SELECT AI PROVIDER.
/// </summary>
public record AnalyzeReceiptCommand(
    
    string UserId,
    string ReceiptId,
    string Prompt,
    string? ProviderName
    
    ) : ICommand<ServiceResult<AIAnalysisResponseDto>>;