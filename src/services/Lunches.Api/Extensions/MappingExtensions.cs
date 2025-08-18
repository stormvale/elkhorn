using Contracts.Lunches.Responses;
using Contracts.Restaurants.Responses;
using Lunches.Api.Domain;

namespace Lunches.Api.Extensions;

public static class MappingExtensions
{
    public static LunchResponse ToLunchResponse(this Lunch lunch)
    {
        var restaurantLunchItems = lunch.AvailableRestaurantItems.Select(x =>
            new LunchItemResponse(
                Name: x.Name,
                Price: x.Price,
                AvailableModifiers: [.. x.AvailableModifiers.Select(m => new LunchItemModifierResponse(m.Name, m.PriceAdjustment))]
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
    
    public static LunchItemModifier ToLunchItemModifier(this RestaurantMealModifierResponse dto) => new(dto.Name, dto.PriceAdjustment);
    
    public static List<LunchItemModifier> ToLunchItemModifiers(this IEnumerable<RestaurantMealModifierResponse> dtos) => [.. dtos.Select(ToLunchItemModifier)];
}
