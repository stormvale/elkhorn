using Contracts.Common;
using Contracts.Lunches.Responses;
using Contracts.Restaurants.Responses;
using Contracts.Schools.Responses;
using Lunches.Api.Domain;
using Shouldly;

namespace Lunches.Api.Tests;

public class LunchTests
{
    [Fact]
    public void NewLunch_ShouldCreateCorrectly()
    {
        var school = new SchoolResponse(
            Id: Guid.CreateVersion7(),
            Name: "Test School", 
            ExternalId: "123",
            new Contact("Principal", "principal@email.com", "Phone", ContactType.Principal),
            new Address("Street", "City", "PostCode", "State"),
            new PacResponse(
                id: Guid.CreateVersion7(),
                new Contact("Pac", "pac@email.com", "Phone", ContactType.Parent), [
                new LunchItemResponse("Gummies", 1.5M, [])
            ]));

        var mcDonalds = new RestaurantResponse(
            Id: Guid.CreateVersion7(),
            Name: "McDonalds",
            new Contact("Manager", "manager@email.com", "Phone", ContactType.Manager),
            new Address("Street", "City", "PostCode", "State"),
            Menu:
            [
                new RestaurantMealResponse(Guid.CreateVersion7(), "Burger", 4.5M, [
                    new RestaurantMealModifierResponse("Extra Cheese", 0.5M)
                ])
            ]);
        
        var lunchDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(14));

        var createLunchResult = Lunch.Create(school, mcDonalds, lunchDate);
        createLunchResult.IsSuccess.ShouldBeTrue();
        
        Lunch lunch = createLunchResult;
        lunch.AvailablePacItems.Count.ShouldBe(1);
        lunch.AvailablePacItems.First().Name.ShouldBe("Gummies");
        
        lunch.AvailableRestaurantItems.Count.ShouldBe(1);
        lunch.AvailableRestaurantItems.First().Name.ShouldBe("Burger");
    }
}