using Contracts.Restaurants.Responses;
using Domain.Common;
using Restaurants.Api.Domain;

namespace Restaurants.Api.Extensions;

public static class MappingExtensions
{
    public static Contact ToDomainContact(this Contracts.Common.Contact dto) => 
        new(dto.Name, dto.Email, dto.Phone, (ContactType)dto.Type);

    public static Address ToDomainAddress(this Contracts.Common.Address dto) =>
        new(dto.Street, dto.City, dto.PostCode, dto.State);
    
    private static Contracts.Common.Contact ToResponse(this Contact contact) => 
        new(contact.Name, contact.Email, contact.Phone, (Contracts.Common.ContactType)contact.Type);

    private static Contracts.Common.Address ToResponse(this Address address) =>
        new(address.Street, address.City, address.PostCode, address.State);
    
    public static RestaurantResponse ToRestaurantResponse(this Restaurant restaurant) => new(
        restaurant.Id,
        restaurant.Name,
        restaurant.Contact.ToResponse(),
        restaurant.Address.ToResponse(),
        restaurant.Menu.Select(meal => meal.ToResponse()).ToList()
    );
    
    private static RestaurantMealResponse ToResponse(this Meal meal) => new(
        meal.Id,
        meal.Name,
        meal.Price,
        meal.AvailableModifiers.Select(mealModifier => mealModifier.ToResponse()).ToList()
    );
    
    private static MealModifier ToMealModifier(this RestaurantMealModifierResponse dto) =>
        new(dto.Name, dto.PriceAdjustment);
    
    private static RestaurantMealModifierResponse ToResponse(this MealModifier mealModifier) =>
        new(mealModifier.Name, mealModifier.PriceAdjustment);
}
