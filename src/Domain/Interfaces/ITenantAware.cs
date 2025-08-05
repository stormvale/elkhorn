namespace Domain.Interfaces;

public interface ITenantAware
{
    string TenantId { get; set; }
}