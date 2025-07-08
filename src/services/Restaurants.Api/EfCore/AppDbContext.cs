using Microsoft.EntityFrameworkCore;
using Restaurants.Api.Domain;

namespace Restaurants.Api.EfCore;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Restaurant> Restaurants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Restaurant>()
            .ToContainer("restaurants")
            .HasPartitionKey(x => x.Id)
            .OwnsMany(r => r.Menu, mealBuilder =>
            {
                mealBuilder.WithOwner();
                mealBuilder.OwnsMany(m => m.AvailableModifiers, modifierBuilder =>
                {
                    modifierBuilder.WithOwner();
                });
            })
            .Property(x => x.Name).HasMaxLength(100);
    }
}


