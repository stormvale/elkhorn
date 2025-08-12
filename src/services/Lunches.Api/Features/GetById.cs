using Contracts.Lunches.Responses;
using Domain.Results;
using Lunches.Api.DomainErrors;
using Lunches.Api.EfCore;
using Lunches.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Lunches.Api.Features;

public static class GetById
{
    public const string RouteName = "GetLunchById";

    public static void MapGetById(this WebApplication app)
    {
        app.MapGet("/{lunchId:Guid}", async (Guid lunchId, AppDbContext db, CancellationToken ct) =>
        {
            var result = await db.Lunches
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == lunchId, ct);

            return result is null
                ? Result.Failure(LunchErrors.NotFound(lunchId)).ToProblemDetails()
                : TypedResults.Ok(result.ToLunchResponse());
        })
        .WithName(RouteName)
        .WithSummary("Get Lunch by Id")
        .WithTags("Lunches")
        .Produces<LunchResponse>()
        .Produces(StatusCodes.Status404NotFound);
}
}