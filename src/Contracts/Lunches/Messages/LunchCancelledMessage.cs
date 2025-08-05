using System.Text.Json.Serialization;

namespace Contracts.Lunches.Messages;

public record LunchDeletedMessage(
    
    [property: JsonPropertyName("lunchId")]
    Guid LunchId);
