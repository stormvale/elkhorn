using Contracts.Schools.Messages;
using Dapr.Client;
using Schools.Api.EfCore;

namespace Schools.Api.Features;

public static class Delete
{
    public static void MapDelete(this WebApplication app)
    {
        app.MapDelete("/{id:Guid}", async Task<IResult> (Guid id, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var restaurant = await db.Schools.FindAsync([id], ct);
            if (restaurant is null)
            {
                return TypedResults.NotFound();
            }
            
            // 'ExecuteDelete' and 'ExecuteDeleteAsync' are not supported by the CosmosDb provider
            db.Schools.Remove(restaurant);
            await db.SaveChangesAsync(ct);

            await dapr.PublishEventAsync("pubsub", "school-events", new SchoolDeletedMessage(id), ct);
            
            return TypedResults.NoContent();
        })
        .WithSummary("Delete");
    }
}