using Contracts.Lunches.Messages;
using Contracts.Restaurants.Messages;
using Dapr.Client;
using Domain.Results;
using Lunches.Api.DomainErrors;
using Lunches.Api.EfCore;
using Lunches.Api.Extensions;

namespace Lunches.Api.Features;

public static class Delete
{
    public static void MapDelete(this WebApplication app)
    {
        app.MapDelete("/{id:Guid}", async (Guid id, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var lunch = await db.Lunches.FindAsync([id], ct);
            if (lunch is null)
            {
                return Result.Failure(LunchErrors.NotFound(id)).ToProblemDetails();
            }
            
            // 'ExecuteDelete' and 'ExecuteDeleteAsync' are not supported by the CosmosDb provider
            db.Lunches.Remove(lunch);
            await db.SaveChangesAsync(ct);

            await dapr.PublishEventAsync("pubsub", "lunch-events", new LunchDeletedMessage(id), ct);
            
            return TypedResults.NoContent();
        })
        .WithName("DeleteLunch")
        .WithSummary("Delete Lunch")
        .WithTags("Lunches")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}