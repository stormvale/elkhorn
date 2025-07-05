using Contracts.Restaurant.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Restaurants.Api.EfCore;
using Restaurants.Api.Extensions;

namespace Restaurants.Api.Features;

public static class GetById
{
    public const string RouteName = "GetById";

    public static void MapGetById(this WebApplication app)
    {
        app.MapGet("/{id:Guid}", async Task<Results<Ok<RestaurantResponse>, NotFound>> (Guid id, AppDbContext db, CancellationToken ct) =>
        {
            var result = await db.Restaurants.FindAsync([id], ct);

            return result is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(result.ToRestaurantResponse());
        })
        .WithName(RouteName)
        .WithSummary("Get by Id");
}
}