using Domain.Results;

namespace Lunches.Api.DomainErrors;

public static class LunchItemErrors
{
    public static DomainError NotFound(Guid lunchItemId, Guid lunchId)
        => DomainError.NotFound("LunchItem.NotFound", $"LunchItem '{lunchItemId}' was not found in Lunch '{lunchId}'.");
}