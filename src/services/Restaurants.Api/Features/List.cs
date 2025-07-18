using Contracts.Restaurants.Responses;
using Microsoft.EntityFrameworkCore;
using Restaurants.Api.EfCore;
using Restaurants.Api.Extensions;

namespace Restaurants.Api.Features;

public static class List
{
    public static void MapList(this WebApplication app)
    {
        app.MapGet("/", async (AppDbContext db, CancellationToken ct) =>
        {
            return await db.Restaurants
                .AsNoTracking()
                .Select(x => x.ToRestaurantResponse()) 
                .ToListAsync(ct);
        })
        .WithName("ListRestaurants")
        .WithSummary("List all Restaurants")
        .WithTags("Restaurants")
        .Produces<List<RestaurantResponse>>();
    }
}