using Contracts.Schools.Pac.Messages;
using Contracts.Schools.Pac.Requests;
using Dapr.Client;
using Domain.Results;
using Microsoft.AspNetCore.Mvc;
using Schools.Api.DomainErrors;
using Schools.Api.EfCore;
using Schools.Api.Extensions;

namespace Schools.Api.Features.Pac;

public static class RemoveLunchItem
{
    public static void MapRemoveLunchItem(this RouteGroupBuilder group)
    {
        group.MapDelete("/lunch-items", async (Guid schoolId, [FromBody] RemoveLunchItemRequest req, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var school = await db.Schools.FindAsync([schoolId], ct);
            if (school is null)
            {
                return Result.Failure(SchoolErrors.NotFound(schoolId)).ToProblemDetails();
            }

            var result = school.Pac.RemoveLunchItem(req.Name);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }

            await db.SaveChangesAsync(ct);
            await dapr.PublishEventAsync("pubsub", "pac-events", new PacLunchItemRemoved(schoolId, req.Name), ct);

            return TypedResults.NoContent();
        })
        .WithName("RemovePacLunchItem")
        .WithSummary("Remove PAC Lunch Item")
        .WithTags("Pac")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}