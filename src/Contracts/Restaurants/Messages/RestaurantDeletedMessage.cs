using System.Text.Json.Serialization;

namespace Contracts.Restaurants.Messages;

public record RestaurantDeletedMessage(
    
    [property: JsonPropertyName("restaurantId")]
    Guid RestaurantId);
