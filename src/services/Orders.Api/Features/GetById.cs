using Contracts.Orders.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Orders.Api.EfCore;
using Orders.Api.Extensions;

namespace Orders.Api.Features;

public static class GetById
{
    public const string RouteName = "GetById";

    public static void MapGetById(this WebApplication app)
    {
        app.MapGet("/{id:Guid}", async Task<Results<Ok<OrderResponse>, NotFound>> (Guid id, AppDbContext db, CancellationToken ct) =>
        {
            var result = await db.Orders.FindAsync([id], ct);

            return result is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(result.ToOrderResponse());
        })
        .WithName(RouteName)
        .WithSummary("Get by Id");
}
}