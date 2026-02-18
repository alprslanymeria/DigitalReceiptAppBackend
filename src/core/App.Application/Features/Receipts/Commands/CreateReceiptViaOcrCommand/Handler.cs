using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Receipt;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.Receipts.Dtos;
using App.Domain.Entities;
using App.Domain.Enums;
using MapsterMapper;

namespace App.Application.Features.Receipts.Commands.CreateReceiptViaOcrCommand;

/// <summary>
/// HANDLER FOR OCR RECEIPT CREATION.
/// USES STRATEGY PATTERN TO EXTRACT RECEIPT DATA FROM IMAGE.
/// </summary>
public class CreateReceiptViaOcrHandler(

    IReceiptStrategyResolver strategyResolver,
    IReceiptRepository receiptRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
    
    ) : ICommandHandler<CreateReceiptViaOcrCommand, ServiceResult<CreateReceiptResponseDto>>
{

    #region UTILS
    private async Task<int?> ResolveOrganizationAsync(ProcessedReceiptData data, CancellationToken ct)
    {
        var existing = await receiptRepository.FindOrganizationByNameAsync(data.OrganizationName!, ct);
        return existing?.Id;
    }

    #endregion

    public async Task<ServiceResult<CreateReceiptResponseDto>> Handle(CreateReceiptViaOcrCommand request, CancellationToken cancellationToken)
    {
        // RESOLVE OCR STRATEGY
        var strategy = strategyResolver.Resolve(SourceType.OCR);

        var context = new ReceiptProcessingContext
        {
            ImageFile = request.ImageFile,
            UserId = request.UserId
        };

        // EXTRACT RECEIPT DATA VIA OCR
        var processedData = await strategy.ExtractReceiptDataAsync(context, cancellationToken);

        // RESOLVE OR CREATE ORGANIZATION
        int? organizationId = null;
        if (!string.IsNullOrWhiteSpace(processedData.OrganizationName))
        {
            organizationId = await ResolveOrganizationAsync(processedData, cancellationToken);
        }

        // CREATE RECEIPT ENTITY
        var receipt = new Receipt
        {
            Id = Guid.NewGuid().ToString(),
            UserId = request.UserId,
            OrganizationId = organizationId,
            TotalAmount = processedData.TotalAmount,
            Currency = processedData.Currency,
            IsFavorite = false,
            Type = SourceType.OCR,
            ImageUrl = processedData.ImageUrl,
            ReceiptItems = processedData.Items.Select(i => new ReceiptItem
            {
                Name = i.Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice,
                TaxRate = i.TaxRate,
                TaxAmount = i.TaxAmount,
                UnitType = i.UnitType,
                UnitSize = i.UnitSize
            }).ToList()
        };

        await receiptRepository.CreateAsync(receipt);
        await unitOfWork.CommitAsync();

        var dto = mapper.Map<CreateReceiptResponseDto>(receipt);
        return ServiceResult<CreateReceiptResponseDto>.SuccessAsCreated(dto, $"/api/v1/receipts/{receipt.Id}");
    }
}
