using System.Text.Json.Serialization;

namespace Contracts.Users.Messages;

public sealed record UserLinkedToSchoolMessage(
    
    [property: JsonPropertyName("userId")]
    Guid UserId,
    
    [property: JsonPropertyName("schoolId")]
    Guid SchoolId);
