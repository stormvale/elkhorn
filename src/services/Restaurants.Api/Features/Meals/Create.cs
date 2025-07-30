using Contracts.Restaurants.Messages;
using Contracts.Restaurants.Requests;
using Contracts.Restaurants.Responses;
using Dapr.Client;
using Domain.Results;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Api.Domain;
using Restaurants.Api.DomainErrors;
using Restaurants.Api.EfCore;
using Restaurants.Api.Extensions;

namespace Restaurants.Api.Features.Meals;

public static class CreateMeal
{
    public static void MapCreateMeal(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (Guid restaurantId, CreateMealRequest req, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var restaurant = await db.Restaurants.FindAsync([restaurantId], ct);
            if (restaurant is null)
            {
                return Result.Failure(RestaurantErrors.NotFound(restaurantId)).ToProblemDetails();
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
        .WithTags("Meals")
        .Produces<CreateMealResponse>()
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }
}