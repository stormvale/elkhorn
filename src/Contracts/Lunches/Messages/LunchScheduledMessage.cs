using ServiceDefaults.MultiTenancy;

namespace Contracts.Lunches.Messages;

public record LunchScheduledMessage(
    Guid LunchId,
    DateOnly Date,
    string SchoolName,
    string RestaurantName,
    string Route = "lunch-scheduled") : ITenantAware
{
    public Guid TenantId { get; set; }
}
