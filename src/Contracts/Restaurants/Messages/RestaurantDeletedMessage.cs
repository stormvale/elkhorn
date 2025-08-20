using ServiceDefaults.Middleware.MultiTenancy;

namespace Contracts.Restaurants.Messages;

public record RestaurantDeletedMessage(Guid RestaurantId) : ITenantAware
{
    public Guid TenantId { get; set; }
}
