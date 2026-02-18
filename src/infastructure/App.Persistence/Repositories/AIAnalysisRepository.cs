using App.Application.Contracts.Persistence.Repositories;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Repositories;

public class AIAnalysisRepository(AppDbContext context) : GenericRepository<AIAnalysis, string>(context), IAIAnalysisRepository
{
    public async Task<List<AIAnalysis>> GetAnalysesByReceiptIdAsync(string receiptId, CancellationToken ct = default)
    {
        return await Context.AIAnalyses
            .AsNoTracking()
            .Where(a => a.ReceiptId == receiptId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(ct);
    }
}
