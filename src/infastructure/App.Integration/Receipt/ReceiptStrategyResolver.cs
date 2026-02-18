using App.Application.Contracts.Infrastructure.Receipt;
using App.Domain.Enums;

namespace App.Integration.Receipt;

/// <summary>
/// RESOLVES THE APPROPRIATE RECEIPT PROCESSING STRATEGY BASED ON SOURCE TYPE.
/// </summary>
public class ReceiptStrategyResolver(IEnumerable<IReceiptProcessingStrategy> strategies) : IReceiptStrategyResolver
{
    private readonly Dictionary<SourceType, IReceiptProcessingStrategy> _strategies =
        strategies.ToDictionary(s => s.SourceType);

    public IReceiptProcessingStrategy Resolve(SourceType sourceType)
    {
        if (!_strategies.TryGetValue(sourceType, out var strategy))
            throw new NotSupportedException($"NO RECEIPT PROCESSING STRATEGY FOUND FOR SOURCE TYPE: {sourceType}");

        return strategy;
    }
}
