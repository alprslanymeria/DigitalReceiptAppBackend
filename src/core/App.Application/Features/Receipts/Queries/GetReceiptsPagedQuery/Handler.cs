using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.Receipts.Dtos;
using MapsterMapper;

namespace App.Application.Features.Receipts.Queries.GetReceiptsPagedQuery;

/// <summary>
/// HANDLER FOR PAGED RECEIPTS QUERY.
/// </summary>
public class GetReceiptsPagedHandler(
    
    IReceiptRepository receiptRepository,
    IMapper mapper
    
    ) : IQueryHandler<GetReceiptsPagedQuery, ServiceResult<PagedResult<ReceiptListDto>>>
{
    public async Task<ServiceResult<PagedResult<ReceiptListDto>>> Handle(GetReceiptsPagedQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await receiptRepository.GetReceiptsPagedAsync(
            request.UserId, request.Page, request.PageSize, cancellationToken);

        var dtos = mapper.Map<List<ReceiptListDto>>(items);

        var pagedRequest = new PagedRequest { Page = request.Page, PageSize = request.PageSize };
        var pagedResult = PagedResult<ReceiptListDto>.Create(dtos, pagedRequest, totalCount);

        return ServiceResult<PagedResult<ReceiptListDto>>.Success(pagedResult);
    }
}