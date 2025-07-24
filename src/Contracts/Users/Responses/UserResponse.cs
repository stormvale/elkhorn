namespace Contracts.Users.Responses;

public sealed record UserResponse(
    string Id,
    string Name,
    string Email,
    string[] SchoolIds,
    uint Version);
