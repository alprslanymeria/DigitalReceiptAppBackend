using App.Application.Contracts.Infrastructure.QrCode;
using App.Domain.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace App.Integration.QrCode;

/// <summary>
/// QR CODE PARSER IMPLEMENTATION.
/// PARSES QR CODE PAYLOADS FROM TURKISH E-FATURA AND OTHER FORMATS.
/// TODO: IMPLEMENT ACTUAL PARSING LOGIC FOR YOUR SPECIFIC QR FORMAT.
/// </summary>
public class QrCodeParser(

    ILogger<QrCodeParser> logger
    
    ) : IQrCodeParser
{

    #region UTILS

    private static bool IsTurkeyEFaturaFormat(string data) =>
        data.Contains("efatura") || data.Contains("gib.gov.tr") || data.StartsWith("MATRAH");

    private static bool IsJsonFormat(string data) =>
        data.TrimStart().StartsWith('{') || data.TrimStart().StartsWith('[');

    /// <summary>
    /// PARSES TURKEY E-FATURA QR CODE FORMAT.
    /// TODO: IMPLEMENT ACTUAL E-FATURA PARSING BASED ON GIB SPECIFICATION.
    /// TYPICAL FORMAT: URL WITH QUERY PARAMS OR STRUCTURED TEXT WITH DELIMITERS.
    /// </summary>
    private QrReceiptData ParseTurkeyEFatura(string data)
    {

        logger.LogWarning("QrCodeParser -> USING PLACEHOLDER TURKEY E-FATURA PARSER");

        return new QrReceiptData
        {
            OrganizationName = null,
            CountryCode = "TR",
            TotalAmount = 0,
            CurrencyCode = "TRY",
            Items = []
        };
    }

    /// <summary>
    /// PARSES JSON-FORMATTED QR CODE DATA.
    /// </summary>
    private QrReceiptData ParseJsonFormat(string data)
    {
        try
        {
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var parsed = JsonSerializer.Deserialize<QrReceiptData>(data, jsonOptions);
            return parsed ?? new QrReceiptData();
        }
        catch (JsonException ex)
        {
            logger.LogWarning(ex, "QrCodeParser -> FAILED TO PARSE JSON QR DATA");
            return new QrReceiptData();
        }
    }

    /// <summary>
    /// PARSES URL-FORMATTED QR CODE (E.G., RECEIPT LOOKUP URL).
    /// TODO: IMPLEMENT URL FETCHING AND PARSING IF QR CONTAINS A URL TO RECEIPT DATA.
    /// </summary>
    private QrReceiptData ParseUrlFormat(string data)
    {
        logger.LogWarning("QrCodeParser -> USING PLACEHOLDER URL FORMAT PARSER FOR: {Data}", data);

        return new QrReceiptData
        {
            TotalAmount = 0,
            CurrencyCode = "TRY",
            Items = []
        };
    }

    #endregion

    public Task<QrReceiptData> ParseAsync(string qrCodeData, CancellationToken ct = default)
    {
        logger.LogInformation("QrCodeParser -> PARSING QR CODE DATA (LENGTH: {Length})", qrCodeData.Length);

        // TRY DIFFERENT FORMATS
        QrReceiptData? result = null;

        result = qrCodeData switch
        {
            var data when IsTurkeyEFaturaFormat(data) => ParseTurkeyEFatura(data),
            var data when IsJsonFormat(data) => ParseJsonFormat(data),
            _ => ParseUrlFormat(qrCodeData),
        };

        logger.LogInformation("QrCodeParser -> PARSED QR. ITEMS: {ItemCount}, TOTAL: {Total}", result.Items.Count, result.TotalAmount);

        return Task.FromResult(result);
    }
}
