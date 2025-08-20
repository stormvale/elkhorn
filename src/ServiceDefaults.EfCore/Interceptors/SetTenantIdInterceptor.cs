using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ServiceDefaults.Middleware;
using ServiceDefaults.Middleware.MultiTenancy;

namespace ServiceDefaults.EfCore.Interceptors;

public sealed class SetTenantIdInterceptor(IRequestContextAccessor requestContext) : SaveChangesInterceptor
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
        var tenantId = requestContext.Current.Tenant.TenantId;
        
        if (context == null || tenantId == Guid.Empty)
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry is { State: EntityState.Added, Entity: ITenantAware tenantAwareEntity })
            {
                tenantAwareEntity.TenantId = tenantId;
            }
        }
    }
}