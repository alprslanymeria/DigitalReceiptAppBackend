using App.Application.Abstractions;
using App.Application.Features.Receipts.DTOs;

namespace App.Application.Features.Receipts.Commands.CreateReceipt;

public sealed class CreateReceiptCommandHandler : ICommandHandler<CreateReceiptCommand, CreateReceiptDto>
{
    public async Task<ServiceResult<CreateReceiptDto>> Handle(CreateReceiptCommand request, CancellationToken cancellationToken)
    {
        // Simulating database operation
        // In a real application, you would use UnitOfWork/Repository to persist the entity
        await Task.CompletedTask;

        var receiptDto = new CreateReceiptDto
        {
            Id = Random.Shared.Next(1, 1000),
            StoreName = request.StoreName,
            TotalAmount = request.TotalAmount
        };

        return ServiceResult<CreateReceiptDto>.Success(receiptDto, System.Net.HttpStatusCode.Created);
    }
}
