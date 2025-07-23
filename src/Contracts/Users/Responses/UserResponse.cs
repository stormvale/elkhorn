using Contracts.Common;

namespace Contracts.Users.Responses;

public sealed record UserResponse(
    string Id,
    string Name,
    Address Address,
    uint Version);
