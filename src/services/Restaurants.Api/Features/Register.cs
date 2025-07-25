﻿using Contracts.Restaurants.Messages;
using Contracts.Restaurants.Requests;
using Contracts.Restaurants.Responses;
using Dapr.Client;
using Restaurants.Api.Domain;
using Restaurants.Api.EfCore;
using Restaurants.Api.Extensions;

namespace Restaurants.Api.Features;

public static class Register
{
    public static void MapRegister(this WebApplication app)
    {
        app.MapPost("/",
            async (RegisterRestaurantRequest req, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
            {
                var restaurant = new Restaurant(Guid.CreateVersion7(),
                    req.Name,
                    req.Address.ToDomainAddress(),
                    req.Contact.ToDomainContact());

                await db.Restaurants.AddAsync(restaurant, ct);
                await db.SaveChangesAsync(ct);

                await dapr.PublishEventAsync("pubsub", "restaurants-events",
                    new RestaurantRegisteredMessage(restaurant.Id, restaurant.Name), ct);

                return TypedResults.CreatedAtRoute(
                    new RegisterRestaurantResponse(restaurant.Id),
                    routeName: GetById.RouteName,
                    routeValues: new { id = restaurant.Id }
                );
            })
        .WithName("RegisterRestaurant")
        .WithSummary("Register new Restaurant")
        .WithTags("Restaurants")
        .RequireAuthorization()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<RegisterRestaurantResponse>(StatusCodes.Status201Created);
    }
}