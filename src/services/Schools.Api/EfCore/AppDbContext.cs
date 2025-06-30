using Microsoft.EntityFrameworkCore;
using Schools.Api.Domain;

namespace Schools.Api.EfCore;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<School> Schools { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<School>().ToContainer("Schools")
            .HasPartitionKey(x => x.Id);

        modelBuilder.Entity<School>().Property(x => x.Name)
            .HasMaxLength(100); // to avoid ef mapping this as ntext or varchar(max)
    }
}


