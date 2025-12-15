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

        if (request.Id <= 0)
        {
            return ServiceResult<ReceiptDto>.Fail("Receipt not found", HttpStatusCode.NotFound);
        }

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
