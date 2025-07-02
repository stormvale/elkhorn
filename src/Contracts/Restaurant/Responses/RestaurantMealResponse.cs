namespace Contracts.Restaurant.Responses;

public record RestaurantMealResponse(
    Guid Id,
    string Name,
    decimal Price,
    List<RestaurantMealModifierResponse> AvailableModifiers);
