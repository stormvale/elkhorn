using Lunches.Api.Domain;
using Lunches.Api.Features.GetById;

namespace Lunches.Api.Extensions;

public static class MappingExtensions
{
    public static LunchResponse ToLunchResponse(this Lunch lunch) => new(
        lunch.Id,
        lunch.SchoolId,
        lunch.RestaurantId,
        lunch.Date,
        lunch.AvailablePacItems.Select(x => new LunchItemResponse(x.Name, x.Price)).ToList()
    );
}