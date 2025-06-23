namespace Lunches.Api.Domain.Events;

public record LunchScheduled(
    Guid LunchId,
    Guid SchoolId,
    Guid RestaurantId,
    DateOnly Date);