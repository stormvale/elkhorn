using ServiceDefaults.Middleware.MultiTenancy;

namespace ServiceDefaults.Middleware;

public class RequestContext
{
    public TenantContext Tenant { get; set; } = new();
    public UserContext User { get; set; } = new();
    public DateTime RequestTimestamp { get; set; } = DateTime.UtcNow;
}