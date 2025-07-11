using Lunches.Api.Domain;
using Shouldly;

namespace Lunches.Api.Tests;

public class LunchTests
{
    [Fact]
    public void NewLunch_ShouldCreateCorrectly()
    {
        var lunch = new Lunch(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateOnly.FromDateTime(DateTime.UtcNow));
        
        lunch.AvailablePacItems.ShouldBeEmpty();
        lunch.AvailableRestaurantItems.ShouldBeEmpty();
    }
}