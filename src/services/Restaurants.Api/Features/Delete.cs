using Contracts.Restaurants.Messages;
using Domain.Results;
using Microsoft.EntityFrameworkCore;
using Restaurants.Api.DomainErrors;
using Restaurants.Api.EfCore;
using Restaurants.Api.Extensions;
using ServiceDefaults.MultiTenancy;

namespace Restaurants.Api.Features;

public static class Delete
{
    public static void MapDelete(this WebApplication app)
    {
        app.MapDelete("/{restaurantId:Guid}", async (Guid restaurantId, AppDbContext db, ITenantAwarePublisher publisher, CancellationToken ct) =>
        {
            var restaurant = await db.Restaurants
                .AsNoTracking()
                .Where(x => x.Id == restaurantId)
                .FirstOrDefaultAsync(ct);
            
            if (restaurant is null)
            {
                return Result.Failure(RestaurantErrors.NotFound(restaurantId)).ToProblemDetails();
            }
            
            // 'ExecuteDelete' and 'ExecuteDeleteAsync' are not supported by the CosmosDb provider
            db.Restaurants.Remove(restaurant);
            await db.SaveChangesAsync(ct);

            await publisher.PublishEventAsync("pubsub", "restaurants-events", new RestaurantDeletedMessage(restaurantId), ct);
            
            return TypedResults.NoContent();
        })
        .WithName("DeleteRestaurant")
        .WithSummary("Delete Restaurant")
        .WithTags("Restaurants")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}