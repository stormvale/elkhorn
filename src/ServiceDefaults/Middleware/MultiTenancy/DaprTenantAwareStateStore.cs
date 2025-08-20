using Dapr.Client;

namespace ServiceDefaults.Middleware.MultiTenancy;

/// <summary>
/// In some cases (eg. dapr pubsub message subscriptions) there is no HttpContext, and therefore
/// no TenantId globally available. Hence, the overloads that accept a TenantId.
/// </summary>
public interface ITenantAwareStateStore
{
    Task SaveStateAsync<T>(string key, T data, CancellationToken ct);
    
    Task SaveStateAsync<T>(string tenantId, string key, T data, CancellationToken ct);

    Task<T?> GetStateAsync<T>(string key, CancellationToken ct);
    
    Task<T?> GetStateAsync<T>(string tenantId, string key, CancellationToken ct);
}

public class DaprTenantAwareStateStore(DaprClient daprClient, IRequestContextAccessor requestContext) : ITenantAwareStateStore
{
    public async Task SaveStateAsync<T>(string key, T data, CancellationToken ct) => 
        await daprClient.SaveStateAsync("statestore", key, data, cancellationToken: ct);

    // TenantId will be used to create a hierarchical key using ':' as a delimiter
    public async Task SaveStateAsync<T>(string tenantId, string key, T data, CancellationToken ct) => 
        await SaveStateAsync($"{tenantId}:{key}", data, ct);

    public async Task<T?> GetStateAsync<T>(string key, CancellationToken ct) => 
        await daprClient.GetStateAsync<T>("statestore", key, ConsistencyMode.Strong, cancellationToken: ct);

    // TenantId will be used to create a hierarchical key using ':' as a delimiter
    public async Task<T?> GetStateAsync<T>(string tenantId, string key, CancellationToken ct) => 
        await GetStateAsync<T>($"{tenantId}:{key}", ct);
}

