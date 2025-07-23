using System.Text.Json.Serialization;

namespace Contracts.Users.Messages;

public sealed record UserRegisteredMessage(
    
    [property: JsonPropertyName("userId")]
    string UserId,
    
    [property: JsonPropertyName("name")]
    string Name);
