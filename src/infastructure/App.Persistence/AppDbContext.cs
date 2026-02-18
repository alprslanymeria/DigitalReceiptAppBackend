using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // DB SETS
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Receipt> Receipts { get; set; }
    public DbSet<ReceiptItem> ReceiptItems { get; set; }
    public DbSet<AIAnalysis> AIAnalyses { get; set; }

    // ON MODEL CREATING
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // APPLY CONFIGURATIONS
        // BU ASSEMBLY İÇERİSİNDE "IEntityTypeConfiguration" INTERFACE'İNİ IMPLEMENT EDEN TÜM CLASS'LARI OTOMATİK OLARAK BULUP UYGULAR
        builder.ApplyConfigurationsFromAssembly(typeof(PersistenceAssembly).Assembly);

        base.OnModelCreating(builder);
    }
}
