using Lunches.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Lunches.Api.EfCore;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Lunch> Lunches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lunch>().ToContainer("lunches")
            .HasPartitionKey(x => x.Id)
            .HasDefaultTimeToLive(null);

        // // not sure if we need this...
        // modelBuilder.Entity<Lunch>()
        //     .HasMany(l => l.AvailablePacItems)
        //     .WithOne();
        //
        // // not sure if we need this...
        // modelBuilder.Entity<Lunch>()
        //     .HasMany(l => l.AvailableRestaurantItems)
        //     .WithOne();

    }
}


