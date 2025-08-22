namespace ServiceDefaults.Middleware.MultiTenancy;

/// <summary>
/// In some cases (eg. dapr pubsub message subscriptions) there is no HttpContext, and therefore
/// no TenantId globally available. Hence, the overloads that accept a TenantId.
/// </summary>
public interface ITenantAwareStateStore
{
    /// <summary>
    /// Saves state data asynchronously with the specified key and data. The key will be prepended
    /// with the TenantId from the current request context => 'tenantId:key'.
    /// </summary>
    /// <typeparam name="T">The type of the data to save.</typeparam>
    /// <param name="key">The key under which the data is stored.</param>
    /// <param name="data">The data to save.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the TenantId is not set in the current request context.</exception>
    Task SaveStateAsync<T>(string key, T data, CancellationToken ct);
    
    /// <summary>
    /// Saves state data asynchronously with the specified key and data. The key will be prepended
    /// with the tenantId provided => 'tenantId:key'.
    /// </summary>
    /// <typeparam name="T">The type of the data to save.</typeparam>
    /// <param name="tenantId">The tenant identifier for the data being stored.</param>
    /// <param name="key">The key under which the data is stored.</param>
    /// <param name="data">The data to save.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SaveStateAsync<T>(Guid tenantId, string key, T data, CancellationToken ct);

    /// <summary>
    /// Retrieves state data asynchronously using the specified key. The key will be prepended
    /// with the TenantId from the current request context => 'tenantId:key'.
    /// </summary>
    /// <typeparam name="T">The type of the data to retrieve.</typeparam>
    /// <param name="key">The key under which the data is stored.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the retrieved data or null if not found.</returns>
    /// /// <exception cref="InvalidOperationException">Thrown when the TenantId is not set in the current request context.</exception>
    Task<T?> GetStateAsync<T>(string key, CancellationToken ct);

    /// <summary>
    /// Retrieves state data asynchronously for the specified tenant, using the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the data to retrieve.</typeparam>
    /// <param name="tenantId">The unique identifier for the tenant.</param>
    /// <param name="key">The key under which the data is stored.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The result is of type T if found, otherwise null.</returns>
    Task<T?> GetStateAsync<T>(Guid tenantId, string key, CancellationToken ct);
}