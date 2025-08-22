namespace ServiceDefaults.Middleware.MultiTenancy;

public interface ITenantAware
{
    Guid TenantId { get; set; }
}