using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ServiceDefaults.EfCore.Interfaces;

namespace ServiceDefaults.EfCore.Interceptors;

internal sealed class UpdateAuditableInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
        {
            UpdateAuditableEntities(eventData.Context);
        }

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            UpdateAuditableEntities(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateAuditableEntities(DbContext context)
    {
        var utcNow = DateTimeOffset.UtcNow;
        var entities = context.ChangeTracker.Entries<IAuditable>().ToList();

        foreach (var entry in entities)
        {
            switch (entry)
            {
                case { State: EntityState.Added }:
                    SetCurrentPropertyValue(entry, nameof(IAuditable.CreatedUtc), utcNow);
                    break;
                case { State: EntityState.Modified }:
                    SetCurrentPropertyValue(entry, nameof(IAuditable.LastModifiedUtc), utcNow);
                    break;
            }
        }

        return;

        static void SetCurrentPropertyValue(EntityEntry entry, string propertyName, DateTimeOffset utcNow) =>
            entry.Property(propertyName).CurrentValue = utcNow;
    }
}
