namespace App.Domain.Options;

/// <summary>
/// CONFIGURATION OPTIONS FOR OCR SERVICE.
/// </summary>
public class OcrConfig
{
    public const string Key = "OcrConfig";

    public string Provider { get; set; } = "GoogleVision";
    public string ApiKey { get; set; } = null!;
    public string Endpoint { get; set; } = null!;
    public int TimeoutSeconds { get; set; } = 30;
    public int MaxImageSizeMB { get; set; } = 10;
    public bool PreprocessingEnabled { get; set; } = true;
}
