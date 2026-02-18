using FluentValidation;

namespace App.Application.Features.Receipts.Commands.CreateReceiptViaOcrCommand;

/// <summary>
/// VALIDATOR FOR OCR RECEIPT CREATION COMMAND
/// </summary>
public class CreateReceiptViaOcrCommandValidator : AbstractValidator<CreateReceiptViaOcrCommand>
{
    private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".webp", ".bmp"];
    private const long MaxImageFileSize = 10 * 1024 * 1024; // 10MB

    public CreateReceiptViaOcrCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("USER ID IS REQUIRED");

        RuleFor(x => x.ImageFile)
            .NotNull()
            .WithMessage("IMAGE FILE IS REQUIRED")
            .Must(file => file is not null && file.Length > 0)
            .WithMessage("IMAGE FILE CANNOT BE EMPTY")
            .Must(file => file is null || file.Length <= MaxImageFileSize)
            .WithMessage($"IMAGE FILE SIZE MUST NOT EXCEED {MaxImageFileSize / 1024 / 1024}MB")
            .Must(file => file is null || AllowedImageExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()))
            .WithMessage($"IMAGE FILE MUST BE ONE OF THE FOLLOWING TYPES: {string.Join(", ", AllowedImageExtensions)}");
    }
}