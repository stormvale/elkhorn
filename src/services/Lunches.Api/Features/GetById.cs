using Contracts.Lunches.Responses;
using Lunches.Api.EfCore;
using Lunches.Api.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Lunches.Api.Features;

public static class GetById
{
    public const string RouteName = "GetById";

    public static void MapGetById(this WebApplication app)
    {
        app.MapGet("/{id:Guid}", async Task<Results<Ok<LunchResponse>, NotFound>> (Guid id, AppDbContext db, CancellationToken ct) =>
        {
            var result = await db.Lunches.FindAsync([id], ct);

            return result is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(result.ToLunchResponse());
        })
        .WithName(RouteName)
        .WithSummary("Get by Id");
}
}