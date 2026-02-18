namespace App.Domain.Options;

/// <summary>
/// CONFIGURATION OPTIONS FOR QR CODE PARSER.
/// </summary>
public class QrCodeConfig
{
    public const string Key = "QrCodeConfig";

    public bool ValidateChecksum { get; set; } = true;
    public string[] SupportedFormats { get; set; } = ["TurkeyEFatura", "Generic"];
}
