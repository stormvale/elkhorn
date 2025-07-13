using Domain.Results;

namespace Restaurants.Api.DomainErrors;

public static class RestaurantErrors
{
    public static DomainError NotFound(Guid id)
        => DomainError.NotFound("Restaurant.NotFound", $"Restaurant with Id '{id}' was not found.");
}