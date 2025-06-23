using Domain.Abstractions;

namespace Restaurants.Api.Domain.Events;

public record RestaurantContactUpdatedDomainEvent(Guid RestaurantId, string ContactName) : DomainEvent(DateTime.UtcNow);
