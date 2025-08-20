using Microsoft.EntityFrameworkCore;
using Schools.Api.Domain;
using ServiceDefaults.Middleware;
using ServiceDefaults.Middleware.MultiTenancy;

namespace Schools.Api.EfCore;

public sealed class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IRequestContextAccessor requestContext) : DbContext(options)
{
    public DbSet<School> Schools { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<School>().ToContainer("schools")
            .HasPartitionKey(x => x.TenantId)
            .HasQueryFilter(x => x.TenantId == requestContext.Current.Tenant.TenantId)
            .HasKey(x => x.Id);

        modelBuilder.Entity<School>().Property(x => x.Name).HasMaxLength(100);
        modelBuilder.Entity<School>().Property(x => x.ExternalId).HasMaxLength(20);

        // owned types are stored as nested JSON inside the parent document
        modelBuilder.Entity<School>().OwnsOne(x => x.Pac);
    }
}


