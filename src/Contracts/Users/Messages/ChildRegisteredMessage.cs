using System.Text.Json.Serialization;

namespace Contracts.Users.Messages;

public sealed record ChildRegisteredMessage(
    
    [property: JsonPropertyName("userId")]
    Guid UserId,
    
    [property: JsonPropertyName("childId")]
    Guid ChildId,
    
    [property: JsonPropertyName("schoolId")]
    Guid SchoolId);
