using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Features.Receipts.CacheKeys;
using App.Application.Features.Receipts.Dtos;

namespace App.Application.Features.Receipts.Queries.SearchReceiptsQuery;

/// <summary>
/// QUERY TO SEARCH RECEIPTS BY TERM
/// </summary>
public record SearchReceiptsQuery(
    
    string UserId,
    string SearchTerm,
    int Page = 1,
    int PageSize = 10

    ) : IQuery<ServiceResult<PagedResult<ReceiptListDto>>>, ICacheableQuery
{
    public ICacheKey GetCacheKey(ICacheKeyFactory keyFactory) => ReceiptCacheKeys.Search(keyFactory, UserId, SearchTerm, Page, PageSize);
}