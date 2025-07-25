namespace Contracts.Users.Responses;

public sealed record UserResponse(
    Guid Id,
    string Name,
    string Email,
    string[] SchoolIds,
    uint Version);
