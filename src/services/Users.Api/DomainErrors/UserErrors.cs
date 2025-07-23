using Domain.Results;

namespace Users.Api.DomainErrors;

public static class UserErrors
{
    public static DomainError NotFound(Guid id)
        => DomainError.NotFound("User.NotFound", $"User with Id '{id}' was not found.");
}