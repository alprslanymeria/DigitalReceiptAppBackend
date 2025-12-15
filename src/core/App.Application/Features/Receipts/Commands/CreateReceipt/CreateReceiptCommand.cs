using App.Application.Abstractions;
using App.Application.Features.Receipts.DTOs;

namespace App.Application.Features.Receipts.Commands.CreateReceipt;

public sealed record CreateReceiptCommand(
    string StoreName,
    decimal TotalAmount,
    DateTime ReceiptDate
) : ICommand<CreateReceiptDto>;
