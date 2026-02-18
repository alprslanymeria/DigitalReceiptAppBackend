using App.Application.Common;
using App.Domain.Entities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IReceiptRepository : IGenericRepository<Receipt, string>
{
    Task<(List<Receipt> Items, int TotalCount)> GetReceiptsPagedAsync(string userId, int page, int pageSize, CancellationToken ct = default);
    Task<Receipt?> GetReceiptWithDetailsAsync(string receiptId, CancellationToken ct = default);
    Task<(string? PreviousId, string? NextId)> GetAdjacentReceiptIdsAsync(string userId, string receiptId, CancellationToken ct = default);
    Task<(List<Receipt> Items, int TotalCount)> SearchReceiptsAsync(string userId, string searchTerm, int page, int pageSize, CancellationToken ct = default);
    Task<Organization?> FindOrganizationByNameAsync(string organizationName, CancellationToken ct = default);
}
