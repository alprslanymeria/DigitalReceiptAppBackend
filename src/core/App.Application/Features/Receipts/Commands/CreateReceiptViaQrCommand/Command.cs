using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.Receipts.Dtos;

namespace App.Application.Features.Receipts.Commands.CreateReceiptViaQrCommand;

/// <summary>
/// COMMAND TO CREATE A RECEIPT VIA QR CODE
/// </summary>
public record CreateReceiptViaQrCommand(

    string UserId,
    string QrCodeData

    ) : ICommand<ServiceResult<CreateReceiptResponseDto>>;