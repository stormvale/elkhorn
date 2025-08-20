using ServiceDefaults.Middleware.MultiTenancy;

namespace Contracts.Lunches.Messages;

public record LunchCancelledMessage(Guid LunchId, string Route = "lunch-cancelled") : ITenantAware
{
    public Guid TenantId { get; set; }
}
