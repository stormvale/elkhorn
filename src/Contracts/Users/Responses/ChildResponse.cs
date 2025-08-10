namespace Contracts.Users.Responses;

public sealed record ChildResponse(
    Guid ChildId,
    string FirstName,
    string LastName,
    Guid ParentId,
    Guid SchoolId,
    string SchoolName,
    string Grade);