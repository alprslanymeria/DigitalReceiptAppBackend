namespace App.Application.Features.Receipts.DTOs;

public class CreateReceiptDto
{
    public int Id { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}
