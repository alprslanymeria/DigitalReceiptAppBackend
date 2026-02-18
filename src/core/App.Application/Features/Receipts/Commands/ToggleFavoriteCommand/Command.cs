using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.Receipts.Dtos;

namespace App.Application.Features.Receipts.Commands.ToggleFavoriteCommand;

/// <summary>
/// COMMAND TO TOGGLE FAVORITE STATUS OF A RECEIPT
/// </summary>
public record ToggleFavoriteCommand(

    string UserId,
    string ReceiptId

    ) : ICommand<ServiceResult<ToggleFavoriteResponseDto>>;