using Domain.Abstractions;

namespace Restaurants.Api.Domain.Events;

// [Topic("restaurant.meal.added")]
public record MealAddedDomainEvent(Guid RestaurantId, Guid MealId) : DomainEvent(DateTime.UtcNow);
