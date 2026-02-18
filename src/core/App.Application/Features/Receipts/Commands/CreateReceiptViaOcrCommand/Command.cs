using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Contracts.Infrastructure.Files;
using App.Application.Features.Receipts.Dtos;

namespace App.Application.Features.Receipts.Commands.CreateReceiptViaOcrCommand;

/// <summary>
/// COMMAND TO CREATE A RECEIPT VIA OCR (IMAGE PROCESSING)
/// </summary>
public record CreateReceiptViaOcrCommand(

    string UserId,
    IFileUpload ImageFile

    ) : ICommand<ServiceResult<CreateReceiptResponseDto>>;