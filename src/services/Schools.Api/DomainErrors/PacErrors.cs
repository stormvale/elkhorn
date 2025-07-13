using Domain.Results;

namespace Schools.Api.DomainErrors;

public static class PacErrors
{
    public static DomainError LunchItemAlreadyExists(string name)
        => DomainError.Conflict("Pac.LunchItemAlreadyExists", $"Lunch item '{name}' already exists.");
}