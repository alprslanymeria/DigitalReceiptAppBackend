using FluentValidation;

namespace App.Application.Features.Receipts.Commands.ToggleFavoriteCommand;

/// <summary>
/// VALIDATOR FOR TOGGLE FAVORITE COMMAND
/// </summary>
public class ToggleFavoriteCommandValidator : AbstractValidator<ToggleFavoriteCommand>
{
    public ToggleFavoriteCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");

        RuleFor(x => x.ReceiptId)
            .NotEmpty()
            .WithMessage("RECEIPT ID IS REQUIRED");
    }
}