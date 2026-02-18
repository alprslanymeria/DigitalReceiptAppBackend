using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Persistence.Repositories;
using App.Application.Features.Receipts.Dtos;
using MapsterMapper;

namespace App.Application.Features.Receipts.Queries.SearchReceiptsQuery;

/// <summary>
/// HANDLER FOR RECEIPT SEARCH QUERY.
/// SEARCHES ACROSS ORGANIZATION NAME AND ITEM NAMES.
/// </summary>
public class SearchReceiptsHandler(
    
    IReceiptRepository receiptRepository,
    IMapper mapper
    
    ) : IQueryHandler<SearchReceiptsQuery, ServiceResult<PagedResult<ReceiptListDto>>>
{
    public async Task<ServiceResult<PagedResult<ReceiptListDto>>> Handle(SearchReceiptsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await receiptRepository.SearchReceiptsAsync(
            request.UserId, request.SearchTerm, request.Page, request.PageSize, cancellationToken);

        var dtos = mapper.Map<List<ReceiptListDto>>(items);

        var pagedRequest = new PagedRequest { Page = request.Page, PageSize = request.PageSize };
        var pagedResult = PagedResult<ReceiptListDto>.Create(dtos, pagedRequest, totalCount);

        return ServiceResult<PagedResult<ReceiptListDto>>.Success(pagedResult);
    }
}