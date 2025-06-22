using Domain.Abstractions;

namespace Restaurants.Api.Domain.Events;

public record RestaurantRegisteredDomainEvent(Guid RestaurantId) : DomainEvent(DateTime.UtcNow);
