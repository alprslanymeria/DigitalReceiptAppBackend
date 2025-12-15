using App.Application.Abstractions;
using App.Application.Features.Receipts.DTOs;

namespace App.Application.Features.Receipts.Queries.GetReceiptById;

public sealed record GetReceiptByIdQuery(int Id) : IQuery<ReceiptDto>;
