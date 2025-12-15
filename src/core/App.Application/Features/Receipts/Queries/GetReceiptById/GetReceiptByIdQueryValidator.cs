using FluentValidation;

namespace App.Application.Features.Receipts.Queries.GetReceiptById;

public sealed class GetReceiptByIdQueryValidator : AbstractValidator<GetReceiptByIdQuery>
{
    public GetReceiptByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Receipt ID must be greater than 0");
    }
}
