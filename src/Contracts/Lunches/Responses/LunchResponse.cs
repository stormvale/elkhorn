namespace Contracts.Lunches.Responses;

public sealed record LunchResponse(
    Guid LunchId,
    Guid SchoolId,
    Guid RestaurantId,
    DateOnly Date,
    List<LunchItemResponse> RestaurantLunchItems,
    List<LunchItemResponse> PacLunchItems);
