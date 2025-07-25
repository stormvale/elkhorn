using Microsoft.EntityFrameworkCore;
using Schools.Api.Domain;

namespace Schools.Api.EfCore;

// TODO: add an IDbContextFactory

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<School> Schools { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<School>().ToContainer("schools")
            .HasPartitionKey(x => x.Id);

        modelBuilder.Entity<School>().Property(x => x.Name).HasMaxLength(100);
        modelBuilder.Entity<School>().Property(x => x.ExternalId).HasMaxLength(20);

        // owned types are stored as nested JSON inside the parent document
        modelBuilder.Entity<School>().OwnsOne(x => x.Pac);
    }
}


