using Contracts.Schools.Messages;
using Dapr.Client;
using Domain.Results;
using Schools.Api.DomainErrors;
using Schools.Api.EfCore;
using Schools.Api.Extensions;

namespace Schools.Api.Features;

public static class Delete
{
    public static void MapDelete(this WebApplication app)
    {
        app.MapDelete("/{id:Guid}",
            async Task<IResult> (Guid id, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
            {
                var school = await db.Schools.FindAsync([id], ct);
                if (school is null)
                {
                    return Result.Failure(SchoolErrors.NotFound(id)).ToProblemDetails();
                }

                // 'ExecuteDelete' and 'ExecuteDeleteAsync' are not supported by the CosmosDb provider
                db.Schools.Remove(school);
                await db.SaveChangesAsync(ct);

                await dapr.PublishEventAsync("pubsub", "schools-events", new SchoolDeletedMessage(id), ct);

                return TypedResults.NoContent();
            })
            .WithName("DeleteSchool")
            .WithSummary("Delete School")
            .WithTags("Schools")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);
    }
}