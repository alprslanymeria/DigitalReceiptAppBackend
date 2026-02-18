namespace App.Domain.Entities;

public class ReceiptItem : AuditableEntity<int>
{
    public string ReceiptId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Quantity { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal UnitPrice { get; set; }
    public string? UnitType { get; set; }
    public decimal? UnitSize { get; set; }
    public string? Category { get; set; } // AI GENERATED
    public string? Brand { get; set; }    // AI GENERATED

    // REFERENCES (PARENTS)
    public Receipt? Receipt { get; set; } // FOR ReceiptId
}
