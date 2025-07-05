using Domain.Common;
using Restaurants.Api.Domain;
using Shouldly;

namespace Restaurants.Api.Tests;

public class RestaurantTests
{
    [Fact]
    public void NewRestaurant_ShouldCreateCorrectly()
    {
        var address = new Address("Street", "City", "PostCode", "State");
        var contact = new Contact("Name", "Email", "Phone", ContactType.Manager);
        var restaurant = new Restaurant(Guid.CreateVersion7(),"Test", address, contact);

        restaurant.Id.ShouldNotBe(Guid.Empty);
        restaurant.Name.ShouldBe("Test");
        restaurant.Address.ShouldBe(address);
        restaurant.Contact.ShouldBe(contact);
        restaurant.Menu.ShouldBeEmpty();
    }
}