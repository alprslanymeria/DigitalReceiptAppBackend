using FluentValidation;

namespace App.Application.Features.Receipts.Commands.CreateReceiptViaQrCommand;

/// <summary>
/// VALIDATOR FOR QR RECEIPT CREATION COMMAND
/// </summary>
public class CreateReceiptViaQrCommandValidator : AbstractValidator<CreateReceiptViaQrCommand>
{
    public CreateReceiptViaQrCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");

        RuleFor(x => x.QrCodeData)
            .NotEmpty()
            .WithMessage("QR CODE DATA IS REQUIRED")
            .MaximumLength(10000)
            .WithMessage("QR CODE DATA MUST NOT EXCEED 10000 CHARACTERS");
    }
}