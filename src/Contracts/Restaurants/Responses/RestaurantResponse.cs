using Contracts.Common;

namespace Contracts.Restaurants.Responses;

public record RestaurantResponse(
    Guid Id,
    string Name,
    Contact Contact,
    Address Address,
    List<RestaurantMealResponse> Menu);
