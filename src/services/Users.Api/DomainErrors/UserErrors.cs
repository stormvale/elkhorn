using Domain.Results;

namespace Users.Api.DomainErrors;

public static class UserErrors
{
    public static DomainError NotFound(Guid userId)
        => DomainError.NotFound("User.NotFound", $"User with Id '{userId}' was not found.");
    
    public static DomainError AlreadyLinkedToSchool(Guid userId, Guid schoolId)
        => DomainError.Conflict("User.AlreadyLinkedToSchool", $"User with Id '{userId}' is already linked to a school with Id '{schoolId}'.");
}