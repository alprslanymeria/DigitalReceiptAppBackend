namespace App.Application.Contracts.Infrastructure.QrCode;

/// <summary>
/// QR CODE PARSER FOR EXTRACTING RECEIPT DATA FROM QR CODE PAYLOADS.
/// </summary>
public interface IQrCodeParser
{
    Task<QrReceiptData> ParseAsync(string qrCodeData, CancellationToken ct = default);
}


/// <summary>
/// PARSED RECEIPT DATA FROM QR CODE
/// </summary>
public record QrReceiptData
{
    public string? OrganizationName { get; init; }
    public string? OrganizationBranch { get; init; }
    public string? CountryCode { get; init; }
    public string? City { get; init; }
    public decimal TotalAmount { get; init; }
    public string? CurrencyCode { get; init; }
    public List<QrReceiptItem> Items { get; init; } = [];
}

public record QrReceiptItem
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