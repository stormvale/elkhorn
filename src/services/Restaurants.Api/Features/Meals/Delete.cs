using Contracts.Restaurants.Messages;
using Domain.Results;
using Microsoft.EntityFrameworkCore;
using Restaurants.Api.DomainErrors;
using Restaurants.Api.EfCore;
using Restaurants.Api.Extensions;
using ServiceDefaults.Middleware.MultiTenancy;

namespace Restaurants.Api.Features.Meals;

public static class DeleteMeal
{
    public static void MapDeleteMeal(this RouteGroupBuilder group)
    {
        group.MapDelete("/{mealId:Guid}", async (Guid restaurantId, Guid mealId, AppDbContext db, ITenantAwarePublisher publisher, CancellationToken ct) =>
        {
            var restaurant = await db.Restaurants
                .Where(x => x.Id == restaurantId)
                .FirstOrDefaultAsync(ct);
            
            if (restaurant is null)
            {
                return Result.Failure(RestaurantErrors.NotFound(restaurantId)).ToProblemDetails();
            }
            
            restaurant.RemoveMeal(mealId);
            await db.SaveChangesAsync(ct);

            await publisher.PublishEventAsync("pubsub", "restaurants-events",
                new RestaurantModifiedMessage(restaurantId), ct);
            
            return TypedResults.NoContent();
        })
        .WithName("DeleteRestaurantMeal")
        .WithSummary("Delete Restaurant Meal")
        .WithTags("Meals")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}