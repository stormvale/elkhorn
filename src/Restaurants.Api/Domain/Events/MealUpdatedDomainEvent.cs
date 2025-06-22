using Domain.Abstractions;

namespace Restaurants.Api.Domain.Events;

// [Topic("restaurant.meal.updated")]
public record MealUpdatedDomainEvent(Guid RestaurantId, Guid MealId) : DomainEvent(DateTime.UtcNow);
