namespace ServiceDefaults.MultiTenancy;

/// <summary>
/// A shared tenant context that is populated early in the request pipeline
/// and is available to downstream services via dependency injection. 
/// </summary>
public sealed class TenantContext
{
    public string TenantId { get; private set; } = string.Empty;
    
    public void SetTenantId(string tenantId) => TenantId = tenantId;
}