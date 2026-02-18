using App.Application.Contracts.Infrastructure.Caching;

namespace App.Application.Features.Receipts.CacheKeys;

/// <summary>
/// CACHE KEY DEFINITIONS FOR RECEIPT ENTITY.
/// </summary>
public static class ReceiptCacheKeys
{
    public static string Prefix => "receipt";
    public static string PagedKey => $"{Prefix}.paged.{{0}}.{{1}}.{{2}}";
    public static string DetailKey => $"{Prefix}.detail.{{0}}";
    public static string SearchKey => $"{Prefix}.search.{{0}}.{{1}}.{{2}}.{{3}}";

    public static ICacheKey Paged(ICacheKeyFactory factory, string userId, int page, int pageSize) =>
        factory.Create(_ => null!, PagedKey, userId, page, pageSize);

    public static ICacheKey Detail(ICacheKeyFactory factory, string receiptId) =>
        factory.Create(_ => null!, DetailKey, receiptId);

    public static ICacheKey Search(ICacheKeyFactory factory, string userId, string searchTerm, int page, int pageSize) =>
        factory.Create(_ => null!, SearchKey, userId, searchTerm, page, pageSize);
}
