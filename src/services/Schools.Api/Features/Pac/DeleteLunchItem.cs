using Contracts.Schools.Pac.Messages;
using Contracts.Schools.Pac.Requests;
using Dapr.Client;
using Domain.Results;
using Schools.Api.DomainErrors;
using Schools.Api.EfCore;
using Schools.Api.Extensions;

namespace Schools.Api.Features.Pac;

public static class DeleteMeal
{
    public static void MapDeleteMeal(this RouteGroupBuilder group)
    {
        group.MapDelete("/pac/lunch-items/", async Task<IResult> (Guid schoolId, RemoveLunchItemRequest req, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
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
        .WithSummary("Remove Lunch Item")
        .WithTags("Pac");
    }
}