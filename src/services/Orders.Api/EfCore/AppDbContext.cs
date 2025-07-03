using Microsoft.EntityFrameworkCore;
using Orders.Api.Domain;

namespace Orders.Api.EfCore;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().ToContainer("orders")
            .HasPartitionKey(x => x.Id)
            .HasDefaultTimeToLive(null);
    }
}


