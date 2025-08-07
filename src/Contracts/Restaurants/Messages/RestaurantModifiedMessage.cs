using ServiceDefaults.MultiTenancy;

namespace Contracts.Restaurants.Messages;

public record RestaurantModifiedMessage(Guid RestaurantId) : ITenantAware
{
    public Guid TenantId { get; set; }
}