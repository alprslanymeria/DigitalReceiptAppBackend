namespace App.Application.Features.Receipts.DTOs;

public class ReceiptDto
{
    public int Id { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime ReceiptDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
