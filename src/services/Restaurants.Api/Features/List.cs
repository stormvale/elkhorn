﻿using Contracts.Restaurants.Responses;
using Microsoft.EntityFrameworkCore;
using Restaurants.Api.EfCore;
using Restaurants.Api.Extensions;

namespace Restaurants.Api.Features;

public static class List
{
    public static void MapList(this WebApplication app)
    {
        app.MapGet("/", async Task<IReadOnlyList<RestaurantResponse>> (AppDbContext db, CancellationToken ct) =>
        {
            var result = await db.Restaurants.ToListAsync(ct);
            
            return result.Select(x => x.ToRestaurantResponse())
                .ToList()
                .AsReadOnly();
            
        })
        .WithSummary("List")
        .WithTags("Restaurants");
    }
}