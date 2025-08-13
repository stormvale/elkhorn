namespace ServiceDefaults.MultiTenancy;

public interface ITenantAware
{
    Guid TenantId { get; set; }
}