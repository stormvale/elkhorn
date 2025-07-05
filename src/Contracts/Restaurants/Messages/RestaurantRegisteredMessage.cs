using System.Text.Json.Serialization;

namespace Contracts.Restaurants.Messages;

public record RestaurantRegisteredMessage(

    [property: JsonPropertyName("restaurantId")]
    Guid RestaurantId,

    [property: JsonPropertyName("name")]
    string Name);
