using Contracts.Lunches.Requests;

namespace Lunches.Api.Features.Schedule;

public record Command(
    Guid LunchId,
    Guid SchoolId,
    Guid RestaurantId,
    DateOnly Date)
{
    public static Command From(Guid lunchId, ScheduleLunchRequest request) => new(
        lunchId,
        request.SchoolId,
        request.RestaurantId,
        request.Date);
}
