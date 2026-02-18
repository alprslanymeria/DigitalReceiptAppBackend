using App.Application.Contracts.Infrastructure.Ocr;
using App.Application.Contracts.Infrastructure.Receipt;
using App.Application.Contracts.Services;
using App.Domain.Enums;

namespace App.Integration.Receipt;

/// <summary>
/// OCR-BASED RECEIPT PROCESSING STRATEGY.
/// PROCESSES RECEIPT IMAGES: PREPROCESSES → OCR → STRUCTURED DATA.
/// </summary>
public class OcrReceiptProcessingStrategy(IOcrService ocrService, IFileStorageHelper fileStorageHelper) : IReceiptProcessingStrategy
{
    public SourceType SourceType => SourceType.OCR;

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
        if (context.ImageFile is null)
            throw new ArgumentNullException(nameof(context.ImageFile), "IMAGE FILE IS REQUIRED FOR OCR PROCESSING");

        // UPLOAD IMAGE TO STORAGE
        var imageUrl = await fileStorageHelper.UploadFileToStorageAsync(context.ImageFile, context.UserId, "receipts");

        // PROCESS IMAGE WITH OCR
        using var stream = context.ImageFile.OpenReadStream();
        var ocrResult = await ocrService.ProcessImageAsync(stream, context.ImageFile.ContentType, ct);

        // MAP CURRENCY
        var currency = ParseCurrency(ocrResult.CurrencyCode);

        return new ProcessedReceiptData
        {
            OrganizationName = ocrResult.OrganizationName,
            OrganizationBranch = ocrResult.OrganizationBranch,
            CountryCode = ocrResult.CountryCode,
            City = ocrResult.City,
            TotalAmount = ocrResult.TotalAmount,
            Currency = currency,
            ImageUrl = imageUrl,
            Items = ocrResult.Items.Select(i => new ProcessedReceiptItemData
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
