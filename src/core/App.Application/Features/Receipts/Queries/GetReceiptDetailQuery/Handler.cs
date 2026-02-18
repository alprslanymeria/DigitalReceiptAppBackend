using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.Receipts.Dtos;
using App.Domain.Exceptions;
using MapsterMapper;

namespace App.Application.Features.Receipts.Queries.GetReceiptDetailQuery;

/// <summary>
/// HANDLER FOR RECEIPT DETAIL QUERY.
/// RETURNS RECEIPT WITH ITEMS, ORGANIZATION, AND ADJACENT RECEIPT IDS FOR NAVIGATION.
/// </summary>
public class GetReceiptDetailHandler(
    
    IReceiptRepository receiptRepository,
    IMapper mapper
    
    ) : IQueryHandler<GetReceiptDetailQuery, ServiceResult<ReceiptDetailDto>>
{
    public async Task<ServiceResult<ReceiptDetailDto>> Handle(GetReceiptDetailQuery request, CancellationToken cancellationToken)
    {
        var receipt = await receiptRepository.GetReceiptWithDetailsAsync(request.ReceiptId, cancellationToken);

        // GUARD: RECEIPT EXISTS
        if (receipt is null)
            throw new NotFoundException($"RECEIPT WITH ID '{request.ReceiptId}' WAS NOT FOUND");

        // GUARD: USER OWNS THE RECEIPT
        if (receipt.UserId != request.UserId)
            throw new BusinessException("YOU ARE NOT AUTHORIZED TO VIEW THIS RECEIPT");

        // GET ADJACENT RECEIPT IDS FOR NAVIGATION (PREV/NEXT)
        var (previousId, nextId) = await receiptRepository.GetAdjacentReceiptIdsAsync(
            request.UserId, request.ReceiptId, cancellationToken);

        var dto = mapper.Map<ReceiptDetailDto>(receipt) with
        {
            PreviousReceiptId = previousId,
            NextReceiptId = nextId
        };

        return ServiceResult<ReceiptDetailDto>.Success(dto);
    }
}