using Contracts.Restaurants.Messages;
using Dapr.Client;
using Domain.Results;
using Restaurants.Api.DomainErrors;
using Restaurants.Api.EfCore;
using Restaurants.Api.Extensions;

namespace Restaurants.Api.Features;

public static class Delete
{
    public static void MapDelete(this WebApplication app)
    {
        app.MapDelete("/{id:Guid}", async (Guid id, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var restaurant = await db.Restaurants.FindAsync([id], ct);
            if (restaurant is null)
            {
                return Result.Failure(RestaurantErrors.NotFound(id)).ToProblemDetails();
            }
            
            // 'ExecuteDelete' and 'ExecuteDeleteAsync' are not supported by the CosmosDb provider
            db.Restaurants.Remove(restaurant);
            await db.SaveChangesAsync(ct);

            await dapr.PublishEventAsync("pubsub", "restaurants-events", new RestaurantDeletedMessage(id), ct);
            
            return TypedResults.NoContent();
        })
        .WithName("DeleteRestaurant")
        .WithSummary("Delete Restaurant")
        .WithTags("Restaurants")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}