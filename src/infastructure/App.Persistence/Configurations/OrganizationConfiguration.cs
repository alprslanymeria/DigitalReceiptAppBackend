using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Persistence.Configurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("Organizations");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(300);
        builder.Property(x => x.Branch).HasMaxLength(300);
        builder.Property(x => x.LogoUrl).HasMaxLength(500);
        builder.Property(x => x.CountryCode).IsRequired().HasMaxLength(10);
        builder.Property(x => x.City).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Region).HasMaxLength(100);

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasMany(x => x.Receipts)
            .WithOne(r => r.Organization)
            .HasForeignKey(r => r.OrganizationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.Name);
    }
}
