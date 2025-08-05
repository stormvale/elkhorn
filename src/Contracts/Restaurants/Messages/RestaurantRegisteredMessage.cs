using ServiceDefaults.MultiTenancy;

namespace Contracts.Restaurants.Messages;

public record RestaurantRegisteredMessage(Guid RestaurantId, string Name) : ITenantAware
{
    public string TenantId { get; set; } = string.Empty;
}
