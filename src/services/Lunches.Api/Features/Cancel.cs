using Contracts.Lunches.Messages;
using Domain.Results;
using Lunches.Api.DomainErrors;
using Lunches.Api.EfCore;
using Lunches.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using ServiceDefaults.Middleware.MultiTenancy;

namespace Lunches.Api.Features;

public static class Cancel
{
    public static void MapCancel(this WebApplication app)
    {
        app.MapDelete("/{lunchId:Guid}", async (Guid lunchId, AppDbContext db, ITenantAwarePublisher publisher, CancellationToken ct) =>
        {
            var lunch = await db.Lunches
                .Where(x => x.Id == lunchId)
                .FirstOrDefaultAsync(ct);
            
            if (lunch is null)
            {
                return Result.Failure(LunchErrors.NotFound(lunchId)).ToProblemDetails();
            }
            
            // 'ExecuteDelete' and 'ExecuteDeleteAsync' are not supported by the CosmosDb provider
            db.Lunches.Remove(lunch);
            await db.SaveChangesAsync(ct);

            await publisher.PublishEventAsync("pubsub", "lunches-events", new LunchCancelledMessage(lunchId), ct);
            
            return TypedResults.NoContent();
        })
        .WithName("CancelLunch")
        .WithSummary("Cancel Lunch")
        .WithTags("Lunches")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}