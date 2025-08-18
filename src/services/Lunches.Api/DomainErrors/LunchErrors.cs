using Domain.Results;

namespace Lunches.Api.DomainErrors;

public static class LunchErrors
{
    public static DomainError NotFound(Guid id)
        => DomainError.NotFound("Lunch.NotFound", $"Lunch with Id '{id}' was not found.");
    
    public static DomainError NoLunchItemsAvailable()
        => DomainError.Failure("Lunch.NoLunchItemsAvailable", $"Could not create Lunch - no lunch items available.");
}