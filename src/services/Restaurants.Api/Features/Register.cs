using Contracts.Restaurants.Messages;
using Contracts.Restaurants.Requests;
using Contracts.Restaurants.Responses;
using Restaurants.Api.Domain;
using Restaurants.Api.EfCore;
using Restaurants.Api.Extensions;
using ServiceDefaults.MultiTenancy;

namespace Restaurants.Api.Features;

public static class Register
{
    public static void MapRegister(this WebApplication app)
    {
        app.MapPost("/", async (RegisterRestaurantRequest req, AppDbContext db, ITenantAwarePublisher publisher, CancellationToken ct) =>
        {
            var restaurant = new Restaurant(Guid.CreateVersion7(),
                req.Name,
                req.Address.ToDomainAddress(),
                req.Contact.ToDomainContact());

            await db.Restaurants.AddAsync(restaurant, ct);
            await db.SaveChangesAsync(ct);
            
            await publisher.PublishEventAsync("pubsub", "restaurants-events",
                new RestaurantRegisteredMessage(restaurant.Id, restaurant.Name), ct);

            return TypedResults.CreatedAtRoute(
                new RegisterRestaurantResponse(restaurant.Id),
                routeName: GetById.RouteName,
                routeValues: new { id = restaurant.Id }
            );
        })
        .WithName("RegisterRestaurant")
        .WithSummary("Register new Restaurant")
        .WithTags("Restaurants")
        .Produces<RegisterRestaurantResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}