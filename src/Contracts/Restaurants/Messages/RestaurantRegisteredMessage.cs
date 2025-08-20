using ServiceDefaults.Middleware.MultiTenancy;

namespace Contracts.Restaurants.Messages;

public record RestaurantRegisteredMessage(Guid RestaurantId, string Name) : ITenantAware
{
    public Guid TenantId { get; set; }
}
