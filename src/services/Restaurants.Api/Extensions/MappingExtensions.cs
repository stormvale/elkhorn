
using Contracts.Common.Responses;
using Contracts.Restaurant.Responses;
using Domain.Common;
using Restaurants.Api.Domain;

namespace Restaurants.Api.Extensions;

public static class MappingExtensions
{
    public static Contact ToContact(this ContactResponse dto) => 
        new(dto.Name, dto.Email, dto.Phone, Enum.Parse<ContactType>(dto.Type));

    public static Address ToAddress(this AddressResponse dto) =>
        new(dto.Street, dto.City, dto.PostCode, dto.State);
    
    private static ContactResponse ToResponse(this Contact contact) => 
        new(contact.Name, contact.Email, contact.Phone, Enum.GetName(contact.Type)!);

    private static AddressResponse ToResponse(this Address address) =>
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
