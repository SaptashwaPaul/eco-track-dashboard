using EcoTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcoTrack.Infrastructure.Persistence;

public class EcoTrackDbContext(DbContextOptions<EcoTrackDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<SupplyNode> SupplyNodes { get; set; }
    public DbSet<Disruption> Disruptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure decimal precision for Carbon Footprint to avoid truncation warnings
        modelBuilder.Entity<Product>()
            .Property(p => p.BaseCarbonFootprintKg)
            .HasPrecision(18, 2);

        // Ensure a node is deleted if its parent product is deleted (Cascade)
        modelBuilder.Entity<SupplyNode>()
            .HasOne(sn => sn.Product)
            .WithMany(p => p.SupplyChain)
            .HasForeignKey(sn => sn.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}