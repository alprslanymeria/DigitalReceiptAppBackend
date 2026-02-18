namespace App.Domain.Entities;

public class Organization : AuditableEntity<int>
{
    public string Name { get; set; } = null!;
    public string? Branch { get; set; }
    public string? LogoUrl { get; set; }
    public string CountryCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string? Region { get; set; }

    // THE REFERENCES THEY GAVE (THEIR CHILDREN)
    public ICollection<Receipt> Receipts { get; set; } = [];
}
