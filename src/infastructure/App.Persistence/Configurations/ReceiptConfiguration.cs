using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Persistence.Configurations;

public class ReceiptConfiguration : IEntityTypeConfiguration<Receipt>
{
    public void Configure(EntityTypeBuilder<Receipt> builder)
    {
        builder.ToTable("Receipts");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasMaxLength(36);

        builder.Property(x => x.UserId).IsRequired().HasMaxLength(128);
        builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
        builder.Property(x => x.Currency).IsRequired();
        builder.Property(x => x.IsFavorite).IsRequired().HasDefaultValue(false);
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.ImageUrl).HasMaxLength(500);
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasMany(x => x.ReceiptItems)
            .WithOne(ri => ri.Receipt)
            .HasForeignKey(ri => ri.ReceiptId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.AIAnalyses)
            .WithOne(a => a.Receipt)
            .HasForeignKey(a => a.ReceiptId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => new { x.UserId, x.CreatedAt });
        builder.HasIndex(x => new { x.UserId, x.IsFavorite });
    }
}
