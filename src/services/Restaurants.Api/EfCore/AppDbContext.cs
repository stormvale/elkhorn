using Microsoft.EntityFrameworkCore;
using Restaurants.Api.Domain;

namespace Restaurants.Api.EfCore;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Restaurant> Restaurants { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
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
    
    // Normally we could add interceptors when adding the DbContext to the WebApp builder,
    // but it seems CosmosDb works a little differently...
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     // Only add interceptor if not already configured and we're in a service scope
    //     if (!optionsBuilder.IsConfigured)
    //     {
    //         return;
    //     }
    //
    //     var serviceProvider = optionsBuilder.Options.FindExtension<CoreOptionsExtension>()?.ApplicationServiceProvider;
    //     if (serviceProvider == null) return;
    //     
    //     // Create a scope to resolve the scoped interceptor
    //     using var scope = serviceProvider.CreateScope();
    //     var interceptor = scope.ServiceProvider.GetService<SetTenantIdInterceptor>();
    //     if (interceptor != null)
    //     {
    //         optionsBuilder.AddInterceptors(interceptor);
    //     }
    // }
}


