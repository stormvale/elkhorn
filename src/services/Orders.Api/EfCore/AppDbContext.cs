using Microsoft.EntityFrameworkCore;
using Orders.Api.Domain;
using ServiceDefaults.Middleware;
using ServiceDefaults.Middleware.MultiTenancy;

namespace Orders.Api.EfCore;

public sealed class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IRequestContextAccessor requestContext) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().ToContainer("orders")
            .HasPartitionKey(x => x.TenantId)
            .HasQueryFilter(x => x.TenantId == requestContext.Current.Tenant.TenantId)
            .HasKey(x => x.Id);
    }
}


