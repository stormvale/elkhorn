using Microsoft.EntityFrameworkCore;
using Restaurants.Api.Domain;

namespace Restaurants.Api.EfCore;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Restaurant> Restaurants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Restaurant>().ToContainer("Restaurant")
            .HasPartitionKey(x => x.Id);

        modelBuilder.Entity<Restaurant>().Property(x => x.Name)
            .HasMaxLength(100); // to avoid ef mapping this as ntext or varchar(max)
    }
}


