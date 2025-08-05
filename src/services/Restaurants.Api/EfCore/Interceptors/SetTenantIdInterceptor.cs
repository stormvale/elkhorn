using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ServiceDefaults.MultiTenancy;

namespace Restaurants.Api.EfCore.Interceptors;

public class SetTenantIdInterceptor(TenantContext tenantContext) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateTenantId(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateTenantId(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateTenantId(DbContext? context)
    {
        if (context == null || string.IsNullOrEmpty(tenantContext.TenantId))
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added && entry.Entity is ITenantAware tenantAwareEntity)
            {
                tenantAwareEntity.TenantId = tenantContext.TenantId;
            }
        }
    }
}