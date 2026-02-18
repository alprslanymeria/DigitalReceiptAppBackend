using App.Domain.Entities;

namespace App.Application.Contracts.Persistence.Repositories;

public interface IAIAnalysisRepository : IGenericRepository<AIAnalysis, string>
{
    Task<List<AIAnalysis>> GetAnalysesByReceiptIdAsync(string receiptId, CancellationToken ct = default);
}
