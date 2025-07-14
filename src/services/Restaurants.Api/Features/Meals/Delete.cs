using Contracts.Restaurants.Messages;
using Dapr.Client;
using Restaurants.Api.EfCore;

namespace Restaurants.Api.Features.Meals;

public static class DeleteMeal
{
    public static void MapDeleteMeal(this RouteGroupBuilder group)
    {
        group.MapDelete("/{mealId:Guid}", async Task<IResult> (Guid restaurantId, Guid mealId, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var restaurant = await db.Restaurants.FindAsync([restaurantId], ct);
            if (restaurant is null)
            {
                return TypedResults.NotFound();
            }
            
            restaurant.RemoveMeal(mealId);
            await db.SaveChangesAsync(ct);

            await dapr.PublishEventAsync("pubsub", "restaurants-events", new RestaurantModifiedMessage(restaurantId), ct);
            
            return TypedResults.NoContent();
        })
        .WithName("DeleteRestaurantMeal")
        .WithSummary("Delete Restaurant Meal")
        .WithTags("Meals");
    }
}