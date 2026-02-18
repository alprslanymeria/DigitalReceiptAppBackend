using App.Domain.Enums;

namespace App.Domain.Entities;

public class Receipt : AuditableEntity<string>
{
    public string UserId { get; set; } = null!;
    public int? OrganizationId { get; set; }
    public decimal TotalAmount { get; set; }
    public Currency Currency { get; set; }
    public bool IsFavorite { get; set; }
    public SourceType Type { get; set; }
    public string? ImageUrl { get; set; }

    // REFERENCES (PARENTS)
    public Organization? Organization { get; set; } // FOR OrganizationId

    // THE REFERENCES THEY GAVE (THEIR CHILDREN)
    public ICollection<ReceiptItem> ReceiptItems { get; set; } = [];
    public ICollection<AIAnalysis> AIAnalyses { get; set; } = [];

}