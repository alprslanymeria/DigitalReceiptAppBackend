using App.Application.Contracts.Infrastructure.Files;

namespace App.Application.Features.Receipts.Dtos;

/// <summary>
/// REQUEST DTO FOR OCR RECEIPT CREATION
/// </summary>
public record CreateReceiptViaOcrRequest(
    
    IFileUpload ImageFile

    );

/// <summary>
/// REQUEST DTO FOR QR RECEIPT CREATION
/// </summary>
public record CreateReceiptViaQrRequest(
    
    string QrCodeData

    );