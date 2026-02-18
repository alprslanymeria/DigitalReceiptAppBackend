using App.Domain.Enums;

namespace App.Application.Contracts.Infrastructure.Receipt;

/// <summary>
/// RESOLVER FOR SELECTING THE APPROPRIATE RECEIPT PROCESSING STRATEGY.
/// </summary>
public interface IReceiptStrategyResolver
{
    IReceiptProcessingStrategy Resolve(SourceType sourceType);
}
