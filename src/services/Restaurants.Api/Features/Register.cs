using Contracts.Restaurants.Messages;
using Contracts.Restaurants.Requests;
using Dapr.Client;
using Restaurants.Api.Domain;
using Restaurants.Api.EfCore;
using Restaurants.Api.Extensions;

namespace Restaurants.Api.Features;

public static class Register
{
    public static void MapRegister(this WebApplication app)
    {
        app.MapPost("/", async (RegisterRestaurantRequest req, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var restaurant = new Restaurant(Guid.CreateVersion7(),
                req.Name,
                req.Address.ToAddress(),
                req.Contact.ToContact());
            
            await db.Restaurants.AddAsync(restaurant, ct);
            await db.SaveChangesAsync(ct);
            
            await dapr.PublishEventAsync("pubsub", "restaurant-events",
                new RestaurantRegisteredMessage(restaurant.Id, restaurant.Name), ct);
            
            return TypedResults.CreatedAtRoute(
                new { restaurantId = restaurant.Id },
                routeName: GetById.RouteName,
                routeValues: new { id = restaurant.Id }
            );
        }).WithSummary("Register");
    }
}