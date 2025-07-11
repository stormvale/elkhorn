using Contracts.Restaurants.Messages;
using Dapr.Client;
using Restaurants.Api.EfCore;

namespace Restaurants.Api.Features;

public static class Delete
{
    public static void MapDelete(this WebApplication app)
    {
        app.MapDelete("/{id:Guid}", async Task<IResult> (Guid id, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var restaurant = await db.Restaurants.FindAsync([id], ct);
            if (restaurant is null)
            {
                return TypedResults.NotFound();
            }
            
            // 'ExecuteDelete' and 'ExecuteDeleteAsync' are not supported by the CosmosDb provider
            db.Restaurants.Remove(restaurant);
            await db.SaveChangesAsync(ct);

            await dapr.PublishEventAsync("pubsub", "restaurants-events", new RestaurantDeletedMessage(id), ct);
            
            return TypedResults.NoContent();
        })
        .WithSummary("Delete")
        .WithTags("Restaurants");
    }
}