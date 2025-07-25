﻿using Domain.Abstractions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Schools.Api.EfCore.Interceptors;

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
            switch (entry.State)
            {
                case EntityState.Added:
                    SetCurrentPropertyValue(entry, nameof(IAuditable.CreatedUtc), utcNow);
                    break;
                case EntityState.Modified:
                    SetCurrentPropertyValue(entry, nameof(IAuditable.LastModifiedUtc), utcNow);
                    break;
            }
        }
        static void SetCurrentPropertyValue(EntityEntry entry, string propertyName, DateTimeOffset utcNow) =>
            entry.Property(propertyName).CurrentValue = utcNow;
    }
}
