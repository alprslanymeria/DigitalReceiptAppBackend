using App.Application.Common;
using App.Application.Common.CQRS;

namespace App.Application.Features.Receipts.Commands.DeleteReceiptCommand;

/// <summary>
/// COMMAND TO DELETE A RECEIPT
/// </summary>
public record DeleteReceiptCommand(

    string UserId,
    string ReceiptId

    ) : ICommand<ServiceResult>;