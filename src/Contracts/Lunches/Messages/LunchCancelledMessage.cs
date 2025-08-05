using System.Text.Json.Serialization;

namespace Contracts.Lunches.Messages;

public record LunchCancelledMessage(
    
    [property: JsonPropertyName("lunchId")]
    Guid LunchId);
