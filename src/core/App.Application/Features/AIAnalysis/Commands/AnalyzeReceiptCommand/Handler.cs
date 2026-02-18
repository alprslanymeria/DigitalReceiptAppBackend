using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.AI;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.AIAnalysis.Dtos;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Exceptions;
using MapsterMapper;
using System.Text.Json;

namespace App.Application.Features.AIAnalysis.Commands.AnalyzeReceiptCommand;

/// <summary>
/// HANDLER FOR AI RECEIPT ANALYSIS.
/// VALIDATES RECEIPT OWNERSHIP, PREPARES RECEIPT DATA AS JSON,
/// AND DELEGATES TO THE SELECTED AI PROVIDER.
/// </summary>
public class AnalyzeReceiptHandler(

    IReceiptRepository receiptRepository,
    IAIAnalysisRepository aiAnalysisRepository,
    IAIProviderFactory aiProviderFactory,
    IUnitOfWork unitOfWork,
    IMapper mapper
    
    ) : ICommandHandler<AnalyzeReceiptCommand, ServiceResult<AIAnalysisResponseDto>>
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    #region UTILS
    private static string SerializeReceiptData(Receipt receipt)
    {
        var data = new
        {
            receipt.TotalAmount,
            Currency = receipt.Currency.ToString(),
            Organization = receipt.Organization?.Name,
            receipt.CreatedAt,
            Items = receipt.ReceiptItems.Select(i => new
            {
                i.Name,
                i.Quantity,
                i.UnitPrice,
                i.TotalPrice,
                i.TaxRate,
                i.TaxAmount,
                i.Category,
                i.Brand
            })
        };

        return JsonSerializer.Serialize(data, JsonOptions);
    }
    #endregion

    public async Task<ServiceResult<AIAnalysisResponseDto>> Handle(AnalyzeReceiptCommand request, CancellationToken cancellationToken)
    {
        // LOAD RECEIPT WITH DETAILS
        var receipt = await receiptRepository.GetReceiptWithDetailsAsync(request.ReceiptId, cancellationToken);

        // GUARD: RECEIPT EXISTS
        if (receipt is null)
            throw new NotFoundException($"RECEIPT WITH ID '{request.ReceiptId}' WAS NOT FOUND");

        // GUARD: USER OWNS THE RECEIPT
        if (receipt.UserId != request.UserId)
            throw new BusinessException("YOU ARE NOT AUTHORIZED TO ANALYZE THIS RECEIPT");

        // RESOLVE AI PROVIDER (STRATEGY PATTERN)
        var providerName = request.ProviderName ?? aiProviderFactory.AvailableProviders.FirstOrDefault()
            ?? throw new BusinessException("NO AI PROVIDER IS CONFIGURED");

        var provider = aiProviderFactory.GetProvider(providerName);

        // PREPARE RECEIPT DATA AS JSON FOR AI CONTEXT
        var receiptDataJson = SerializeReceiptData(receipt);

        // CREATE ANALYSIS RECORD
        var analysis = new Domain.Entities.AIAnalysis
        {
            Id = Guid.NewGuid().ToString(),
            ReceiptId = request.ReceiptId,
            Type = AnalysisType.SPENDING_INSIGHT,
            Status = AnalysisStatus.Pending,
            ModelProvider = provider.ProviderName,
            ModelName = string.Empty,
            InputJson = JsonSerializer.Serialize(new { request.Prompt, ReceiptData = receiptDataJson }, JsonOptions),
            RetryCount = 0
        };

        try
        {
            // CALL AI PROVIDER
            var result = await provider.AnalyzeAsync(request.Prompt, receiptDataJson, cancellationToken);

            // UPDATE ANALYSIS WITH RESULT
            analysis.Status = AnalysisStatus.Completed;
            analysis.OutputJson = result.OutputJson;
            analysis.ModelName = result.ModelName;
            analysis.ModelVersion = result.ModelVersion;
            analysis.TokenUsage = result.TokenUsage;
            analysis.CostUsd = result.CostUsd;
        }
        catch (Exception ex)
        {
            analysis.Status = AnalysisStatus.Failed;
            analysis.ErrorMessage = ex.Message;
        }

        await aiAnalysisRepository.CreateAsync(analysis);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<AIAnalysisResponseDto>(analysis);
        return ServiceResult<AIAnalysisResponseDto>.Success(dto);
    }

    
}
