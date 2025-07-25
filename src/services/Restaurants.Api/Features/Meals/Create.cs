﻿using Contracts.Restaurants.Messages;
using Contracts.Restaurants.Requests;
using Contracts.Restaurants.Responses;
using Dapr.Client;
using Microsoft.AspNetCore.Http.HttpResults;
using Restaurants.Api.Domain;
using Restaurants.Api.EfCore;

namespace Restaurants.Api.Features.Meals;

public static class CreateMeal
{
    public static void MapCreateMeal(this RouteGroupBuilder group)
    {
        group.MapPost("/", async Task<Results<Ok<CreateMealResponse>, ProblemHttpResult>> (Guid restaurantId, CreateMealRequest req, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var restaurant = await db.Restaurants.FindAsync([restaurantId], ct);
            if (restaurant is null)
            {
                return TypedResults.Problem(
                    detail: $"Restaurant with Id {restaurantId} was not found",
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Restaurant not found");
            }
            
            var meal = new Meal(Guid.CreateVersion7(), req.Name, req.Price);
            req.Modifiers.ForEach(x => meal.AddModifier(x.Name, x.PriceAdjustment));
            restaurant.AddMeal(meal);
            
            await db.SaveChangesAsync(ct);
            
            await dapr.PublishEventAsync("pubsub", "restaurants-events",
                new RestaurantModifiedMessage(restaurant.Id), ct);

            return TypedResults.Ok(new CreateMealResponse(meal.Id, restaurant.Id));
        })
        .WithName("CreateRestaurantMeal")
        .WithSummary("Create Restaurant Meal")
        .WithTags("Meals");
    }
}