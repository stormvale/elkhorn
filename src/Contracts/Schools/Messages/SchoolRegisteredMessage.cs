using System.Text.Json.Serialization;

namespace Contracts.Schools.Messages;

public sealed record SchoolRegisteredMessage(
    
    [property: JsonPropertyName("schoolId")]
    Guid SchoolId,
    
    [property: JsonPropertyName("name")]
    string Name);
