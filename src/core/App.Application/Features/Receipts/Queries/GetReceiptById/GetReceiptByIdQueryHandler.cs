using System.Net;
using App.Application.Abstractions;
using App.Application.Features.Receipts.DTOs;

namespace App.Application.Features.Receipts.Queries.GetReceiptById;

public sealed class GetReceiptByIdQueryHandler : IQueryHandler<GetReceiptByIdQuery, ReceiptDto>
{
    public async Task<ServiceResult<ReceiptDto>> Handle(GetReceiptByIdQuery request, CancellationToken cancellationToken)
    {
        // Simulating database operation
        // In a real application, you would use UnitOfWork/Repository to fetch the entity
        await Task.CompletedTask;

        // In a real scenario, if receipt not found in database, return NotFound
        // For now, returning sample data
        var receiptDto = new ReceiptDto
        {
            Id = request.Id,
            StoreName = "Sample Store",
            TotalAmount = 99.99m,
            ReceiptDate = DateTime.Now.AddDays(-1),
            CreatedAt = DateTime.Now
        };

        return ServiceResult<ReceiptDto>.Success(receiptDto);
    }
}
