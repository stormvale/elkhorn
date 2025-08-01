using Contracts.Lunches.Responses;
using Domain.Results;
using Lunches.Api.DomainErrors;
using Lunches.Api.EfCore;
using Lunches.Api.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Lunches.Api.Features;

public static class GetById
{
    public const string RouteName = "GetLunchById";

    public static void MapGetById(this WebApplication app)
    {
        app.MapGet("/{id:Guid}", async (Guid id, AppDbContext db, CancellationToken ct) =>
        {
            var result = await db.Lunches.FindAsync([id], ct);

            return result is null
                ? Result.Failure(LunchErrors.NotFound(id)).ToProblemDetails()
                : TypedResults.Ok(result.ToLunchResponse());
        })
        .WithName(RouteName)
        .WithSummary("Get Lunch by Id")
        .WithTags("Lunches")
        .Produces<LunchResponse>()
        .Produces(StatusCodes.Status404NotFound);
}
}