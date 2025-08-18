using Contracts.Lunches.Messages;
using Contracts.Lunches.Requests;
using Contracts.Lunches.Responses;
using Contracts.Restaurants.Responses;
using Contracts.Schools.Responses;
using Lunches.Api.Domain;
using Lunches.Api.EfCore;
using Lunches.Api.Extensions;
using ServiceDefaults.MultiTenancy;

namespace Lunches.Api.Features;

public static class Schedule
{
    public static void MapSchedule(this WebApplication app)
    {
        app.MapPost("/", async (ScheduleLunchRequest req, AppDbContext db, ITenantAwareServiceInvoker invoker, ITenantAwarePublisher publisher, CancellationToken ct) =>
        {
            var school = await invoker.InvokeMethodAsync<SchoolResponse>(
                HttpMethod.Get, "schools-api", $"/{req.SchoolId}", ct);

            var restaurant = await invoker.InvokeMethodAsync<RestaurantResponse>(
                HttpMethod.Get, "restaurants-api", $"/{req.RestaurantId}", ct);

            Lunch lunch = Lunch.Create(school, restaurant, req.Date);
            
            await db.Lunches.AddAsync(lunch, ct);
            await db.SaveChangesAsync(ct);
            
            await publisher.PublishEventAsync("pubsub", "lunches-events", 
                new LunchScheduledMessage(lunch.Id, lunch.Date, school.Name, restaurant.Name), ct);
            
            return TypedResults.CreatedAtRoute(
                lunch.ToLunchResponse(),
                routeName: GetById.RouteName,
                routeValues: new { lunchId = lunch.Id }
            );
        })
        .WithName("ScheduleLunch")
        .WithSummary("Schedule new Lunch")
        .WithTags("Lunches")
        .Produces<LunchResponse>(StatusCodes.Status201Created);
    }
}