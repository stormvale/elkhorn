using Dapr.Client;

namespace ServiceDefaults.Middleware.MultiTenancy;

public class DaprTenantAwareStateStore(DaprClient daprClient, IRequestContextAccessor requestContext) : ITenantAwareStateStore
{
    /// <inheritdoc />
    public async Task SaveStateAsync<T>(string key, T data, CancellationToken ct)
    {
        var tenantId = requestContext.Current.Tenant.TenantId;
        if (tenantId == Guid.Empty)
        {
            throw new InvalidOperationException("TenantId is not set in the current request context.");
        }

        await SaveStateAsync(tenantId, key, data, ct);
    }

    /// <inheritdoc />
    public async Task SaveStateAsync<T>(Guid tenantId, string key, T data, CancellationToken ct) =>
        await daprClient.SaveStateAsync("statestore", $"{tenantId}:{key}", data, cancellationToken: ct);

    /// <inheritdoc />
    public async Task<T?> GetStateAsync<T>(string key, CancellationToken ct)
    {
        var tenantId = requestContext.Current.Tenant.TenantId;
        if (tenantId == Guid.Empty)
        {
            throw new InvalidOperationException("TenantId is not set in the current request context.");
        }
        
        return await GetStateAsync<T>(tenantId, key, ct);
    }

    /// <inheritdoc />
    public async Task<T?> GetStateAsync<T>(Guid tenantId, string key, CancellationToken ct) =>
        await daprClient.GetStateAsync<T>("statestore", $"{tenantId}:{key}", ConsistencyMode.Strong, cancellationToken: ct);
}