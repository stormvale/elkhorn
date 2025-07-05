using Contracts.Lunches.Responses;
using Lunches.Api.Domain;

namespace Lunches.Api.Extensions;

public static class MappingExtensions
{
    public static LunchResponse ToLunchResponse(this Lunch lunch)
    {
        var restaurantLunchItems = lunch.AvailableRestaurantItems.Select(x =>
            new LunchItemResponse(
                x.Name,
                x.Price,
                x.AvailableModifiers.Select(m => new LunchItemModifierResponse(m.Name, m.PriceAdjustment)
                ).ToList()
            )
        ).ToList();
            
        return new LunchResponse(
            lunch.Id,
            lunch.SchoolId,
            lunch.RestaurantId,
            lunch.Date,
            restaurantLunchItems,
            lunch.AvailablePacItems.Select(x => new LunchItemResponse(x.Name, x.Price, [])).ToList()
        );
    }
}
