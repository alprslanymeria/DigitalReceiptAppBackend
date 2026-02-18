using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Persistence.Configurations;

public class AIAnalysisConfiguration : IEntityTypeConfiguration<AIAnalysis>
{
    public void Configure(EntityTypeBuilder<AIAnalysis> builder)
    {
        builder.ToTable("AIAnalyses");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasMaxLength(36);

        builder.Property(x => x.ReceiptId).IsRequired().HasMaxLength(36);
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.ModelProvider).IsRequired().HasMaxLength(100);
        builder.Property(x => x.ModelName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.ModelVersion).HasMaxLength(50);
        builder.Property(x => x.InputJson).HasColumnType("nvarchar(max)");
        builder.Property(x => x.OutputJson).HasColumnType("nvarchar(max)");
        builder.Property(x => x.ErrorMessage).HasMaxLength(2000);
        builder.Property(x => x.RetryCount).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.CostUsd).HasPrecision(10, 6);
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasIndex(x => x.ReceiptId);
        builder.HasIndex(x => x.Status);
    }
}
