namespace ServiceDefaults.MultiTenancy;

public interface ITenantAware
{
    string TenantId { get; set; }
}