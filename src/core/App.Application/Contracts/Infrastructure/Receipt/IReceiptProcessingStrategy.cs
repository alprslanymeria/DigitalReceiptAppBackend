using App.Application.Contracts.Infrastructure.Files;
using App.Domain.Enums;

namespace App.Application.Contracts.Infrastructure.Receipt;

/// <summary>
/// STRATEGY INTERFACE FOR RECEIPT PROCESSING.
/// EACH IMPLEMENTATION HANDLES A DIFFERENT SOURCE TYPE (OCR, QR).
/// </summary>
public interface IReceiptProcessingStrategy
{
    SourceType SourceType { get; }
    Task<ProcessedReceiptData> ExtractReceiptDataAsync(ReceiptProcessingContext context, CancellationToken ct = default);
}

/// <summary>
/// PROCESSED RECEIPT DATA FROM ANY SOURCE
/// </summary>
public record ProcessedReceiptData
{
    public string? OrganizationName { get; init; }
    public string? OrganizationBranch { get; init; }
    public string? CountryCode { get; init; }
    public string? City { get; init; }
    public decimal TotalAmount { get; init; }
    public Currency Currency { get; init; }
    public string? ImageUrl { get; init; }
    public List<ProcessedReceiptItemData> Items { get; init; } = [];
}

public record ProcessedReceiptItemData
{
    public string Name { get; init; } = null!;
    public decimal Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal TotalPrice { get; init; }
    public decimal TaxRate { get; init; }
    public decimal TaxAmount { get; init; }
    public string? UnitType { get; init; }
    public decimal? UnitSize { get; init; }
}

/// <summary>
/// CONTEXT CARRYING ALL POSSIBLE INPUT DATA FOR RECEIPT PROCESSING.
/// </summary>
public record ReceiptProcessingContext
{
    public IFileUpload? ImageFile { get; init; }
    public string? QrCodeData { get; init; }
    public string UserId { get; init; } = null!;
}