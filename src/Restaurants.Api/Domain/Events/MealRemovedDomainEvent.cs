using Domain.Abstractions;

namespace Restaurants.Api.Domain.Events;

// [Topic("restaurant.meal.removed")]
public record MealRemovedDomainEvent(Guid RestaurantId, Guid MealId) : DomainEvent(DateTime.UtcNow);
