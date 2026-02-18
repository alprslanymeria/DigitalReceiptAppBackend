using FluentValidation;

namespace App.Application.Features.Receipts.Queries.GetReceiptDetailQuery;

/// <summary>
/// VALIDATOR FOR GET RECEIPT DETAIL QUERY
/// </summary>
public class GetReceiptDetailQueryValidator : AbstractValidator<GetReceiptDetailQuery>
{
    public GetReceiptDetailQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");

        RuleFor(x => x.ReceiptId)
            .NotEmpty()
            .WithMessage("RECEIPT ID IS REQUIRED");
    }
}