using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Restaurants.Api.Extensions;

public static class EfCoreExtensions
{
    /// <summary>
    /// EF Core’s FindAsync does not currently support partition key lookup for Cosmos DB. Even though
    /// Cosmos DB requires both id and partitionKey for a point read, EF Core’s FindAsync only uses
    /// the primary key, and there's no overload that accepts a partition key.
    ///
    /// This was an attempt to first check the change tracker for the entity and then fall back to a
    /// query with partition key. Wasn't really getting anywhere :-/
    /// </summary>
    public static async Task<T?> FindWithPartitionAsync<T>(
        this DbSet<T> dbSet,
        Guid id,
        CancellationToken ct = default) where T: class, IAggregateRoot<Guid>
    {
        var infrastructure = dbSet as IInfrastructure<IServiceProvider>;
        var serviceProvider = infrastructure.Instance;
        var currentDbContext = serviceProvider.GetService<ICurrentDbContext>();
        
        var tracked = currentDbContext?.Context.ChangeTracker.Entries<T>()
            .FirstOrDefault(e => e.Entity.Id == id)?.Entity;
        
        if (tracked is not null)
            return tracked;
        
        // doesn't work: 'WithPartitionKey' can only be called on an entity query root
        return await dbSet
            .WithPartitionKey(id)
            .FirstOrDefaultAsync(r => r.Id == id, ct);
    }
}