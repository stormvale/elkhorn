using Lunches.Api.Domain;
using Microsoft.EntityFrameworkCore;
using ServiceDefaults.Middleware;

namespace Lunches.Api.EfCore;

public sealed class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IRequestContextAccessor requestContext) : DbContext(options)
{
    public DbSet<Lunch> Lunches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultContainer("lunches");
        
        modelBuilder.Entity<Lunch>().ToContainer("lunches")
            .HasPartitionKey(x => x.TenantId)
            .HasQueryFilter(x => x.TenantId == requestContext.Current.Tenant.TenantId)
            .OwnsMany(x => x.AvailablePacItems, builder => builder.WithOwner())
            .OwnsMany(x => x.AvailableRestaurantItems, builder => builder.WithOwner())
            .HasKey(x => x.Id);
        
        //modelBuilder.Entity<LunchItem>().Property(x => x.Name).HasMaxLength(100);
    }
}


