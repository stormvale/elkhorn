﻿using Microsoft.EntityFrameworkCore;
using Users.Api.Domain;

namespace Users.Api.EfCore;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToContainer("users")
            .HasPartitionKey(x => x.Id);

        modelBuilder.Entity<User>().Property(x => x.Name).HasMaxLength(100);
        modelBuilder.Entity<User>().Property(x => x.Email).HasMaxLength(100);
        
        modelBuilder.Entity<User>().OwnsMany(r => r.Children, childBuilder =>
        {
            childBuilder.WithOwner();
            childBuilder.Property(x => x.FirstName).HasMaxLength(50);
            childBuilder.Property(x => x.LastName).HasMaxLength(50);
            childBuilder.Property(x => x.Grade).HasMaxLength(20);
            childBuilder.Property(x => x.ParentId).HasMaxLength(20);
        });
    }
}


