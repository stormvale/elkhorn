using Lunches.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Lunches.Api.EfCore;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Lunch> Lunches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lunch>().ToContainer("lunches")
            .HasPartitionKey(x => x.PartitionKey)
            //.HasQueryFilter(x => x.TenantId == tenantContext.TenantId)
            .HasKey(x => x.Id);
    }
}


