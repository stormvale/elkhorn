using Contracts.Restaurants.Responses;
using Domain.Results;
using Restaurants.Api.DomainErrors;
using Restaurants.Api.EfCore;
using Restaurants.Api.Extensions;

namespace Restaurants.Api.Features;

public static class GetById
{
    public const string RouteName = "GetById";

    public static void MapGetById(this WebApplication app)
    {
        app.MapGet("/{id:Guid}", async (Guid id, AppDbContext db, CancellationToken ct) =>
        {
            var restaurant = await db.Restaurants.FindAsync([id], ct);
            
            return restaurant is null 
                ? Result.Failure(RestaurantErrors.NotFound(id)).ToProblemDetails()
                : TypedResults.Ok(restaurant.ToRestaurantResponse());
        })
        .WithName(RouteName)
        .WithSummary("Get by Id")
        .WithTags("Restaurants")
        .Produces<RestaurantResponse>()
        .Produces(StatusCodes.Status404NotFound);
    }
}