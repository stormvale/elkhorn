using Contracts.Common.Responses;

namespace Contracts.Restaurant.Responses;

public record RestaurantResponse(
    Guid Id,
    string Name,
    ContactResponse Contact,
    AddressResponse Address,
    List<MealResponse> Menu,
    uint Version,
    DateTimeOffset CreatedUtc,
    DateTimeOffset? LastModifiedUtc);
