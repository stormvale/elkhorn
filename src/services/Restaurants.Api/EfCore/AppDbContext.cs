using Microsoft.EntityFrameworkCore;
using Restaurants.Api.Domain;
using ServiceDefaults.Middleware;

namespace Restaurants.Api.EfCore;

// TODO: IDbContextFactory?

public sealed class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IRequestContextAccessor requestContext) : DbContext(options)
{
    public DbSet<Restaurant> Restaurants { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Restaurant>()
            .ToContainer("restaurants")
            .HasPartitionKey(x => x.TenantId)
            .HasQueryFilter(x => x.TenantId == requestContext.Current.Tenant.TenantId)
            .OwnsMany(r => r.Menu, mealBuilder =>
            {
                mealBuilder.WithOwner();
                mealBuilder.OwnsMany(m => m.AvailableModifiers, modifierBuilder =>
                {
                    modifierBuilder.WithOwner();
                });
            })
            .HasKey(x => x.Id);
        
        modelBuilder.Entity<Restaurant>()
            .Property(x => x.Name).HasMaxLength(100);
    }
}


