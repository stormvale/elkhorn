using Contracts.Lunches.Responses;
using Contracts.Restaurants.Responses;
using Lunches.Api.Domain;

namespace Lunches.Api.Extensions;

public static class MappingExtensions
{
    public static LunchResponse ToLunchResponse(this Lunch lunch)
    {
        var restaurantLunchItems = lunch.AvailableRestaurantItems.Select(x => x.ToLunchItemResponse()).ToList();
        var pacLunchItems = lunch.AvailablePacItems.Select(x => new LunchItemResponse(x.Id, x.Name, x.Price, [])).ToList();

        return new LunchResponse(
            lunch.Id,
            lunch.SchoolId,
            lunch.RestaurantId,
            lunch.Date,
            restaurantLunchItems,
            pacLunchItems
        );
    }

    public static LunchItemResponse ToLunchItemResponse(this LunchItem li) => new(
        Id: li.Id,
        Name: li.Name,
        Price: li.Price,
        AvailableModifiers:
        [.. li.AvailableModifiers.Select(m => new LunchItemModifierResponse(m.Name, m.PriceAdjustment))]
    );
    
    public static LunchItemModifier ToLunchItemModifier(this RestaurantMealModifierResponse dto) => new(dto.Name, dto.PriceAdjustment);
    
    public static List<LunchItemModifier> ToLunchItemModifiers(this IEnumerable<RestaurantMealModifierResponse> dtos) => [.. dtos.Select(ToLunchItemModifier)];
}
