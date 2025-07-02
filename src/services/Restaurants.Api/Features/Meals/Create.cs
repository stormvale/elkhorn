using Contracts.Restaurant.Messages;
using Contracts.Restaurant.Requests;
using Dapr.Client;
using Microsoft.AspNetCore.Http.HttpResults;
using Restaurants.Api.Domain;
using Restaurants.Api.EfCore;

namespace Restaurants.Api.Features.Meals;

public static class CreateMeal
{
    public static void MapCreateMeal(this RouteGroupBuilder group)
    {
        group.MapPost("/", async Task<Results<Ok, ProblemHttpResult>> (Guid restaurantId, CreateMealRequest req, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var restaurant = await db.Restaurants.FindAsync([restaurantId], ct);
            if (restaurant is null)
            {
                return TypedResults.Problem(
                    detail: $"Restaurant with Id {restaurantId} was not found",
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Restaurant not found");
            }
            
            var meal = new Meal(Guid.CreateVersion7(), req.Name, req.Price);
            req.Modifiers.ForEach(x => meal.AddModifier(x.Name, x.PriceAdjustment));
            restaurant.AddMeal(meal);
            
            await db.SaveChangesAsync(ct);
            
            await dapr.PublishEventAsync("pubsub", "restaurant-events",
                new RestaurantMenuModifiedMessage(restaurant.Id), ct);
            
            return TypedResults.Ok();
        });
    }
}