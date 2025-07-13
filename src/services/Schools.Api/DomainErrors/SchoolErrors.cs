using Domain.Results;

namespace Schools.Api.DomainErrors;

public static class SchoolErrors
{
    public static DomainError NotFound(Guid id)
        => DomainError.NotFound("School.NotFound", $"School with Id '{id}' was not found.");
}