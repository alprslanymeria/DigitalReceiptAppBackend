using FluentValidation;

namespace App.Application.Features.Receipts.Commands.DeleteReceiptCommand;

/// <summary>
/// VALIDATOR FOR DELETE RECEIPT COMMAND
/// </summary>
public class DeleteReceiptCommandValidator : AbstractValidator<DeleteReceiptCommand>
{
    public DeleteReceiptCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");

        RuleFor(x => x.ReceiptId)
            .NotEmpty()
            .WithMessage("RECEIPT ID IS REQUIRED");
    }
}
