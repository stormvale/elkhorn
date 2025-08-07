using Contracts.Lunches.Messages;
using Contracts.Lunches.Requests;
using Contracts.Lunches.Responses;
using Contracts.Restaurants.Responses;
using Contracts.Schools.Responses;
using Dapr.Client;
using Lunches.Api.Domain;
using Lunches.Api.EfCore;
using Lunches.Api.Extensions;

namespace Lunches.Api.Features;

public static class Schedule
{
    public static void MapSchedule(this WebApplication app)
    {
        app.MapPost("/", async (ScheduleLunchRequest req, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var school = await dapr.InvokeMethodAsync<SchoolResponse>(
                HttpMethod.Get, "schools-api", $"/{req.SchoolId}", ct);
            
            // I think this is failing because we are calling the service directly. When services are called via the
            // api gateway, it automatically sets the TenantId on the TenantContext, which is later used in the global
            // query filter in all AppDbContext's, to handle multi-tenancy.
            //
            // If the service is called directly without passing the TenantId, it won't be available on the other end.
            var restaurant = await dapr.InvokeMethodAsync<RestaurantResponse>(
                HttpMethod.Get, "restaurants-api", $"/{req.RestaurantId}", ct);

            var pacLunchItems = school.Pac.LunchItems.Select(x =>
                new LunchItem(
                    Name: x.Name,
                    Price: x.Price,
                    AvailableModifiers: [])
            ).ToList();

            var restaurantLunchItems = restaurant.Menu.Select(x =>
                new LunchItem(
                    Name: x.Name,
                    Price: x.Price,
                    AvailableModifiers: [.. x.AvailableModifiers.Select(y => new LunchItemModifier(y.Name, y.PriceAdjustment))])
            ).ToList();
            
            var lunch = new Lunch(Guid.CreateVersion7(), req.SchoolId, req.RestaurantId, req.Date);
            lunch.AddPacItems(pacLunchItems);
            lunch.AddRestaurantItems(restaurantLunchItems);
            
            await db.Lunches.AddAsync(lunch, ct);
            await db.SaveChangesAsync(ct);
            
            await dapr.PublishEventAsync("pubsub", "lunches-events",
                new LunchScheduledMessage(lunch.Id, lunch.Date, school.Name, restaurant.Name), ct);
            
            return TypedResults.CreatedAtRoute(
                lunch.ToLunchResponse(),
                routeName: GetById.RouteName,
                routeValues: new { id = lunch.Id }
            );
        })
        .WithName("ScheduleLunch")
        .WithSummary("Schedule new Lunch")
        .WithTags("Lunches")
        .Produces<LunchResponse>(StatusCodes.Status201Created);
    }
}