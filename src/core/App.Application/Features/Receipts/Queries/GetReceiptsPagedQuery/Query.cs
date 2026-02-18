using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Features.Receipts.CacheKeys;
using App.Application.Features.Receipts.Dtos;

namespace App.Application.Features.Receipts.Queries.GetReceiptsPagedQuery;

/// <summary>
/// QUERY TO GET PAGED RECEIPTS FOR A USER
/// </summary>
public record GetReceiptsPagedQuery(
    
    string UserId,
    int Page = 1,
    int PageSize = 10

    ) : IQuery<ServiceResult<PagedResult<ReceiptListDto>>>, ICacheableQuery
{
    public ICacheKey GetCacheKey(ICacheKeyFactory keyFactory) => ReceiptCacheKeys.Paged(keyFactory, UserId, Page, PageSize);
}