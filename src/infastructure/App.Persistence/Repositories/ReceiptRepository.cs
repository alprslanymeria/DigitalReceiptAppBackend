using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

public class ReceiptRepository(AppDbContext context) : GenericRepository<Receipt, string>(context), IReceiptRepository
{
    public async Task<(List<Receipt> Items, int TotalCount)> GetReceiptsPagedAsync(string userId, int page, int pageSize, CancellationToken ct = default)
    {
        var query = Context.Receipts
            .AsNoTracking()
            .Include(r => r.Organization)
            .Include(r => r.ReceiptItems)
            .Where(r => r.UserId == userId);

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task<Receipt?> GetReceiptWithDetailsAsync(string receiptId, CancellationToken ct = default)
    {
        return await Context.Receipts
            .AsNoTracking()
            .Include(r => r.Organization)
            .Include(r => r.ReceiptItems)
            .FirstOrDefaultAsync(r => r.Id == receiptId, ct);
    }

    public async Task<(string? PreviousId, string? NextId)> GetAdjacentReceiptIdsAsync(string userId, string receiptId, CancellationToken ct = default)
    {
        var currentReceipt = await Context.Receipts
            .AsNoTracking()
            .Where(r => r.Id == receiptId)
            .Select(r => r.CreatedAt)
            .FirstOrDefaultAsync(ct);

        if (currentReceipt == default)
            return (null, null);

        // PREVIOUS = OLDER RECEIPT (CREATED BEFORE CURRENT)
        var previousId = await Context.Receipts
            .AsNoTracking()
            .Where(r => r.UserId == userId && r.CreatedAt < currentReceipt)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => r.Id)
            .FirstOrDefaultAsync(ct);

        // NEXT = NEWER RECEIPT (CREATED AFTER CURRENT)
        var nextId = await Context.Receipts
            .AsNoTracking()
            .Where(r => r.UserId == userId && r.CreatedAt > currentReceipt)
            .OrderBy(r => r.CreatedAt)
            .Select(r => r.Id)
            .FirstOrDefaultAsync(ct);

        return (previousId, nextId);
    }

    public async Task<(List<Receipt> Items, int TotalCount)> SearchReceiptsAsync(string userId, string searchTerm, int page, int pageSize, CancellationToken ct = default)
    {
        var normalizedTerm = searchTerm.Trim().ToLower();

        var query = Context.Receipts
            .AsNoTracking()
            .Include(r => r.Organization)
            .Include(r => r.ReceiptItems)
            .Where(r => r.UserId == userId &&
                (
                    (r.Organization != null && r.Organization.Name.Contains(normalizedTerm, StringComparison.CurrentCultureIgnoreCase)) ||
                    r.ReceiptItems.Any(ri => ri.Name.Contains(normalizedTerm, StringComparison.CurrentCultureIgnoreCase)) ||
                    r.ReceiptItems.Any(ri => ri.Category != null && ri.Category.Contains(normalizedTerm, StringComparison.CurrentCultureIgnoreCase)) ||
                    r.ReceiptItems.Any(ri => ri.Brand != null && ri.Brand.Contains(normalizedTerm, StringComparison.CurrentCultureIgnoreCase))
                ));

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task<Organization?> FindOrganizationByNameAsync(string organizationName, CancellationToken ct = default)
    {
        return await Context.Organizations
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Name == organizationName, ct);
    }
}
