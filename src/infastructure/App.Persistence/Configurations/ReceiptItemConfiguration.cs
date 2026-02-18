using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Persistence.Configurations;

public class ReceiptItemConfiguration : IEntityTypeConfiguration<ReceiptItem>
{
    public void Configure(EntityTypeBuilder<ReceiptItem> builder)
    {
        builder.ToTable("ReceiptItems");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.ReceiptId).IsRequired().HasMaxLength(36);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(300);
        builder.Property(x => x.Quantity).HasPrecision(18, 4);
        builder.Property(x => x.TaxRate).HasPrecision(5, 2);
        builder.Property(x => x.TaxAmount).HasPrecision(18, 2);
        builder.Property(x => x.TotalPrice).HasPrecision(18, 2);
        builder.Property(x => x.UnitPrice).HasPrecision(18, 2);
        builder.Property(x => x.UnitType).HasMaxLength(50);
        builder.Property(x => x.UnitSize).HasPrecision(18, 4);
        builder.Property(x => x.Category).HasMaxLength(100);
        builder.Property(x => x.Brand).HasMaxLength(200);
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasIndex(x => x.ReceiptId);
        builder.HasIndex(x => x.Name);
    }
}
