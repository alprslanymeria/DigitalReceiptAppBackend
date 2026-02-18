using FluentValidation;

namespace App.Application.Features.AIAnalysis.Commands.AnalyzeReceiptCommand;

/// <summary>
/// VALIDATOR FOR ANALYZE RECEIPT COMMAND
/// </summary>
public class AnalyzeReceiptCommandValidator : AbstractValidator<AnalyzeReceiptCommand>
{
    public AnalyzeReceiptCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");

        RuleFor(x => x.ReceiptId)
            .NotEmpty()
            .WithMessage("RECEIPT ID IS REQUIRED");

        RuleFor(x => x.Prompt)
            .NotEmpty()
            .WithMessage("PROMPT IS REQUIRED")
            .MaximumLength(2000)
            .WithMessage("PROMPT MUST NOT EXCEED 2000 CHARACTERS");
    }
}