using Domain.Results;

namespace Users.Api.DomainErrors;

public static class UserErrors
{
    public static DomainError NotFound(Guid userId)
        => DomainError.NotFound("User.NotFound", $"User with Id '{userId}' was not found.");
    
    public static DomainError ChildAlreadyRegistered(Guid userId, string childName)
        => DomainError.Conflict("User.ChildAlreadyRegistered", $"Child with name {childName} already is already registered to user with Id '{userId}'.");
    
    public static DomainError ChildNotFound(Guid userId, Guid childId)
        => DomainError.NotFound("User.ChildNotFound", $"Child with Id '{childId}' was not found for user with Id '{userId}'.");
    
    public static DomainError CreateChildFailed(string childFirstName)
        => DomainError.Failure("User.CreateChildFailed", $"Could not create Child ({childFirstName}).");
    
}