using FluentValidation;

namespace App.Application.Features.Receipts.Commands.CreateReceipt;

public sealed class CreateReceiptCommandValidator : AbstractValidator<CreateReceiptCommand>
{
    public CreateReceiptCommandValidator()
    {
        RuleFor(x => x.StoreName)
            .NotEmpty().WithMessage("Store name is required")
            .MaximumLength(200).WithMessage("Store name must not exceed 200 characters");

        RuleFor(x => x.TotalAmount)
            .GreaterThan(0).WithMessage("Total amount must be greater than 0");

        RuleFor(x => x.ReceiptDate)
            .NotEmpty().WithMessage("Receipt date is required")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Receipt date cannot be in the future");
    }
}
