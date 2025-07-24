using Domain.Results;

namespace Users.Api.DomainErrors;

public static class UserErrors
{
    public static DomainError NotFound(string userId)
        => DomainError.NotFound("User.NotFound", $"User with Id '{userId}' was not found.");
    
    public static DomainError AlreadyLinkedToSchool(string userId, Guid schoolId)
        => DomainError.Conflict("User.AlreadyLinkedToSchool", $"User with Id '{userId}' is already linked to a school with Id '{schoolId}'.");
}