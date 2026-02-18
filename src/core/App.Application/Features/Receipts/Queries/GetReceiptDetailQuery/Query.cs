using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Caching;
using App.Application.Features.Receipts.CacheKeys;
using App.Application.Features.Receipts.Dtos;

namespace App.Application.Features.Receipts.Queries.GetReceiptDetailQuery;

/// <summary>
/// QUERY TO GET RECEIPT DETAIL WITH ITEMS AND NAVIGATION
/// </summary>
public record GetReceiptDetailQuery(
    
    string UserId,
    string ReceiptId
    
    ) : IQuery<ServiceResult<ReceiptDetailDto>>, ICacheableQuery
{
    public ICacheKey GetCacheKey(ICacheKeyFactory keyFactory) => ReceiptCacheKeys.Detail(keyFactory, ReceiptId);
}