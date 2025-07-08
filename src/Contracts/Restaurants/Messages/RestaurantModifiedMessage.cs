using System.Text.Json.Serialization;

namespace Contracts.Restaurants.Messages;

public record RestaurantModifiedMessage(
    
    [property: JsonPropertyName("restaurantId")]
    Guid RestaurantId);