using Contracts.Lunches.Messages;
using Contracts.Lunches.Requests;
using Contracts.Restaurants.Responses;
using Contracts.Schools.Messages;
using Contracts.Schools.Responses;
using Dapr.Client;
using Lunches.Api.Domain;
using Lunches.Api.EfCore;

namespace Lunches.Api.Features;

public static class Schedule
{
    public static void MapSchedule(this WebApplication app)
    {
        app.MapPost("/", async (ScheduleLunchRequest req, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var school = await dapr.InvokeMethodAsync<SchoolResponse>(
                HttpMethod.Get, "schools-api", $"/{req.SchoolId}", ct);
            
            var restaurant = await dapr.InvokeMethodAsync<RestaurantResponse>(
                HttpMethod.Get, "restaurants-api", $"/{req.RestaurantId}", ct);

            var pacLunchItems = school.Pac.LunchItems.Select(x =>
                new LunchItem(
                    x.Name,
                    x.Price,
                    [])
            ).ToList();

            var restaurantLunchItems = restaurant.Menu.Select(x =>
                new LunchItem(
                    x.Name,
                    x.Price,
                    x.AvailableModifiers.Select(y => new LunchItemModifier(y.Name, y.PriceAdjustment)).ToList())
            ).ToList();
            
            var lunch = new Lunch(Guid.CreateVersion7(), req.SchoolId, req.RestaurantId, req.Date);
            lunch.AddPacItems(pacLunchItems);
            lunch.AddRestaurantItems(restaurantLunchItems);
            
            await db.Lunches.AddAsync(lunch, ct);
            await db.SaveChangesAsync(ct);
            
            await dapr.PublishEventAsync("pubsub", "lunch-events",
                new LunchScheduledMessage(lunch.Id, lunch.Date, school.Name, restaurant.Name), ct);
            
            return TypedResults.CreatedAtRoute(
                lunch.Id,
                routeName: GetById.RouteName,
                routeValues: new { id = lunch.Id }
            );
        }).WithSummary("Schedule");
    }
}