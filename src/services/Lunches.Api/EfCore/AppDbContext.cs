using Lunches.Api.Domain;
using Microsoft.EntityFrameworkCore;
using ServiceDefaults.MultiTenancy;

namespace Lunches.Api.EfCore;

public sealed class AppDbContext(
    DbContextOptions<AppDbContext> options,
    TenantContext tenantContext) : DbContext(options)
{
    public DbSet<Lunch> Lunches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lunch>().ToContainer("lunches")
            .HasPartitionKey(x => x.PartitionKey)
            .HasQueryFilter(x => x.TenantId == tenantContext.TenantId)
            .OwnsMany(x => x.AvailablePacItems, builder =>
            {
                builder.WithOwner();
                // builder.OwnsMany(m => m.AvailableModifiers, modifierBuilder =>
                // {
                //     modifierBuilder.WithOwner();
                // });
            })
            .OwnsMany(x => x.AvailableRestaurantItems, builder =>
            {
                builder.WithOwner();
            })
            .HasKey(x => x.Id);
    }
}


