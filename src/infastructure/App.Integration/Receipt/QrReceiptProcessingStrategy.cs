using App.Application.Contracts.Infrastructure.QrCode;
using App.Domain.Enums;

namespace App.Application.Contracts.Infrastructure.Receipt;

/// <summary>
/// QR CODE-BASED RECEIPT PROCESSING STRATEGY.
/// PARSES QR CODE PAYLOAD INTO STRUCTURED RECEIPT DATA.
/// </summary>
public class QrReceiptProcessingStrategy(IQrCodeParser qrCodeParser) : IReceiptProcessingStrategy
{
    public SourceType SourceType => SourceType.QR;

    #region UTILS

    private static Currency ParseCurrency(string? currencyCode) => currencyCode?.ToUpperInvariant() switch
    {
        "TRY" or "TL" => Currency.TRY,
        "USD" => Currency.USD,
        "EUR" => Currency.EUR,
        "GBP" => Currency.GBP,
        _ => Currency.TRY
    };

    #endregion

    public async Task<ProcessedReceiptData> ExtractReceiptDataAsync(ReceiptProcessingContext context, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(context.QrCodeData))
            throw new ArgumentNullException(nameof(context.QrCodeData), "QR CODE DATA IS REQUIRED FOR QR PROCESSING");

        var qrResult = await qrCodeParser.ParseAsync(context.QrCodeData, ct);

        var currency = ParseCurrency(qrResult.CurrencyCode);

        return new ProcessedReceiptData
        {
            OrganizationName = qrResult.OrganizationName,
            OrganizationBranch = qrResult.OrganizationBranch,
            CountryCode = qrResult.CountryCode,
            City = qrResult.City,
            TotalAmount = qrResult.TotalAmount,
            Currency = currency,
            ImageUrl = null,
            Items = qrResult.Items.Select(i => new ProcessedReceiptItemData
            {
                Name = i.Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice,
                TaxRate = i.TaxRate,
                TaxAmount = i.TaxAmount,
                UnitType = i.UnitType,
                UnitSize = i.UnitSize
            }).ToList()
        };
    }
}
