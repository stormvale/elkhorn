using Contracts.Schools.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Schools.Api.EfCore;
using Schools.Api.Extensions;

namespace Schools.Api.Features;

public static class GetById
{
    public const string RouteName = "GetById";

    public static void MapGetById(this WebApplication app)
    {
        app.MapGet("/{id:Guid}", async Task<Results<Ok<SchoolResponse>, NotFound>> (Guid id, AppDbContext db, CancellationToken ct) =>
        {
            var result = await db.Schools.FindAsync([id], ct);

            return result is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(result.ToSchoolResponse());
        })
        .WithName(RouteName)
        .WithSummary("Get by Id");
}
}