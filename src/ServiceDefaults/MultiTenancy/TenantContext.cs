namespace ServiceDefaults.MultiTenancy;

/// <summary>
/// A shared tenant context that is populated early in the request pipeline
/// and is available to downstream services via dependency injection. 
/// </summary>
public sealed class TenantContext
{
    public Guid TenantId { get; private set; }
    
    public void SetTenantId(Guid tenantId) => TenantId = tenantId;
}