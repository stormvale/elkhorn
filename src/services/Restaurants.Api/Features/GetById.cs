using Contracts.Restaurants.Responses;
using Domain.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurants.Api.DomainErrors;
using Restaurants.Api.EfCore;
using Restaurants.Api.Extensions;

namespace Restaurants.Api.Features;

public static class GetById
{
    public const string RouteName = "GetRestaurantById";

    public static void MapGetById(this WebApplication app)
    {
        app.MapGet("/{restaurantId:Guid}", async (Guid restaurantId, AppDbContext db, CancellationToken ct) =>
        {
            var restaurant = await db.Restaurants
                .AsNoTracking()
                .Where(x => x.Id == restaurantId)
                .FirstOrDefaultAsync(ct);
            
            return restaurant is null 
                ? Result.Failure(RestaurantErrors.NotFound(restaurantId)).ToProblemDetails()
                : TypedResults.Ok(restaurant.ToRestaurantResponse());
        })
        .WithName(RouteName)
        .WithSummary("Get Restaurant by Id")
        .WithTags("Restaurants")
        .Produces<RestaurantResponse>()
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }
}