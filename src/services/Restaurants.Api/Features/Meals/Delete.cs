using Contracts.Restaurants.Messages;
using Dapr.Client;
using Domain.Results;
using Restaurants.Api.DomainErrors;
using Restaurants.Api.EfCore;
using Restaurants.Api.Extensions;

namespace Restaurants.Api.Features.Meals;

public static class DeleteMeal
{
    public static void MapDeleteMeal(this RouteGroupBuilder group)
    {
        group.MapDelete("/{mealId:Guid}", async (Guid restaurantId, Guid mealId, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var restaurant = await db.Restaurants.FindAsync([restaurantId], ct);
            if (restaurant is null)
            {
                return Result.Failure(RestaurantErrors.NotFound(restaurantId)).ToProblemDetails();
            }
            
            restaurant.RemoveMeal(mealId);
            await db.SaveChangesAsync(ct);

            await dapr.PublishEventAsync("pubsub", "restaurants-events", new RestaurantModifiedMessage(restaurantId), ct);
            
            return TypedResults.NoContent();
        })
        .WithName("DeleteRestaurantMeal")
        .WithSummary("Delete Restaurant Meal")
        .WithTags("Meals")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}