using System.Text.Json.Serialization;

namespace Contracts.Restaurant.Messages;

public record RestaurantRegisteredMessage(

    [property: JsonPropertyName("restaurantId")]
    Guid RestaurantId,

    [property: JsonPropertyName("name")]
    string Name);
