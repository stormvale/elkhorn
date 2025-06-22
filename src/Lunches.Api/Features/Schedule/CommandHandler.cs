using Contracts.Lunches.Messages;
using Contracts.Restaurant.Responses;
using Contracts.Schools.Responses;
using Lunches.Api.Domain;
using Lunches.Api.Domain.Events;
using Marten;
using Wolverine;
using Wolverine.Marten;

namespace Lunches.Api.Features.Schedule;

[AggregateHandler]
public static class CommandHandler
{
    /// This is an example of Wolverine's "compound handlers" feature, where separate methods are used for different
    /// concerns (like loading data or validating data) so that the "main" message handler (or HTTP endpoint method)
    /// can be a pure function that is completely focused on domain logic or business workflow logic for easy reasoning
    /// and effective unit testing.
    public static async Task<(SchoolResponse, RestaurantResponse)> LoadAsync(Command command, IHttpClientFactory clientFactory, CancellationToken ct)
    {
        // these should both probably use caching

        var school = await clientFactory.CreateClient("schools")
            .GetFromJsonAsync<SchoolResponse>($"api/schools/{command.SchoolId}", ct);
        
        var restaurant = await clientFactory.CreateClient("restaurants")
            .GetFromJsonAsync<RestaurantResponse>($"api/restaurants/{command.RestaurantId}", ct);

        return (school, restaurant)!;
    }

    public static (Events, OutgoingMessages) Handle(Command command, Lunch _, SchoolResponse school, RestaurantResponse restaurant)
    {
        // the SchoolResponse and RestaurantResponse objects are passed in here from the previously-executed LoadAsync method.

        if (restaurant.Menu.IsEmpty())
        {
            throw new ArgumentException($"Menu for restaurant {restaurant.Name} cannot be empty.");
        }

        var pacLunchItems = school.Pac.LunchItems.Select(x => new LunchItem(x.Name, x.Price, [])).ToList();
        var restaurantLunchItems = restaurant.Menu.Select(x =>
            new LunchItem(
                x.Name,
                x.Price,
                x.AvailableModifiers.Select(y =>
                    new LunchItemModifier(y.Name, y.PriceAdjustment)).ToList())
        ).ToList();

        // events will be appended to the stream
        var events = new Events
        {
            new LunchScheduled(command.LunchId, school.Id, restaurant.Id, command.Date),
            new ItemsAddedToLunch(pacLunchItems.Concat(restaurantLunchItems).ToList())
        };

        // outgoing messages will be cascaded
        OutgoingMessages messages = [new LunchScheduledMessage(command.LunchId)];

        return (events, messages);
    }
}