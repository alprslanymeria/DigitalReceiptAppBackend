using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Receipt;
using App.Application.Contracts.Persistence;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.Receipts.Dtos;
using App.Domain.Entities;
using App.Domain.Enums;
using MapsterMapper;

namespace App.Application.Features.Receipts.Commands.CreateReceiptViaQrCommand;

/// <summary>
/// HANDLER FOR QR RECEIPT CREATION.
/// USES STRATEGY PATTERN TO EXTRACT RECEIPT DATA FROM QR CODE.
/// </summary>
public class CreateReceiptViaQrHandler(

    IReceiptStrategyResolver strategyResolver,
    IReceiptRepository receiptRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
    
    ) : ICommandHandler<CreateReceiptViaQrCommand, ServiceResult<CreateReceiptResponseDto>>
{
    public async Task<ServiceResult<CreateReceiptResponseDto>> Handle(CreateReceiptViaQrCommand request, CancellationToken cancellationToken)
    {
        // RESOLVE QR STRATEGY
        var strategy = strategyResolver.Resolve(SourceType.QR);

        var context = new ReceiptProcessingContext
        {
            QrCodeData = request.QrCodeData,
            UserId = request.UserId
        };

        // EXTRACT RECEIPT DATA VIA QR
        var processedData = await strategy.ExtractReceiptDataAsync(context, cancellationToken);

        // RESOLVE ORGANIZATION
        int? organizationId = null;
        if (!string.IsNullOrWhiteSpace(processedData.OrganizationName))
        {
            var existing = await receiptRepository.FindOrganizationByNameAsync(processedData.OrganizationName, cancellationToken);
            organizationId = existing?.Id;
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
            Type = SourceType.QR,
            ImageUrl = null,
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
