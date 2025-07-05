using Contracts.Common.Responses;

namespace Contracts.Restaurants.Responses;

public record RestaurantResponse(
    Guid Id,
    string Name,
    ContactResponse Contact,
    AddressResponse Address,
    List<RestaurantMealResponse> Menu);
