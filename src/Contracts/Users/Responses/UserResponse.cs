namespace Contracts.Users.Responses;

public sealed record UserResponse(
    Guid Id,
    string Name,
    string Email,
    ChildResponse[] Children,
    Guid[] SchoolIds,
    uint Version);
