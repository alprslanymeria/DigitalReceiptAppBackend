namespace App.Application.Contracts.Infrastructure.Ocr;


/// <summary>
/// OCR SERVICE FOR EXTRACTING RECEIPT DATA FROM IMAGES.
/// IMPLEMENTATIONS SHOULD HANDLE IMAGE PREPROCESSING FOR OPTIMAL OCR ACCURACY.
/// </summary>
public interface IOcrService
{
    Task<OcrResult> ProcessImageAsync(Stream imageStream, string contentType, CancellationToken ct = default);
}

/// <summary>
/// RESULT OF OCR PROCESSING ON A RECEIPT IMAGE
/// </summary>
public record OcrResult
{
    public string? OrganizationName { get; init; }
    public string? OrganizationBranch { get; init; }
    public string? CountryCode { get; init; }
    public string? City { get; init; }
    public decimal TotalAmount { get; init; }
    public string? CurrencyCode { get; init; }
    public List<OcrReceiptItem> Items { get; init; } = [];
}

public record OcrReceiptItem
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
