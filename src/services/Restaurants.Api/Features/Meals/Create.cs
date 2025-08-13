using Contracts.Restaurants.Messages;
using Contracts.Restaurants.Requests;
using Contracts.Restaurants.Responses;
using Domain.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurants.Api.Domain;
using Restaurants.Api.DomainErrors;
using Restaurants.Api.EfCore;
using Restaurants.Api.Extensions;
using ServiceDefaults.MultiTenancy;

namespace Restaurants.Api.Features.Meals;

public static class CreateMeal
{
    public static void MapCreateMeal(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (Guid restaurantId, CreateMealRequest req, AppDbContext db, ITenantAwarePublisher publisher, CancellationToken ct) =>
        {
            var restaurant = await db.Restaurants
                .Where(x => x.Id == restaurantId)
                .FirstOrDefaultAsync(ct);
            
            if (restaurant is null)
            {
                return Result.Failure(RestaurantErrors.NotFound(restaurantId)).ToProblemDetails();
            }
            
            var meal = new Meal(Guid.CreateVersion7(), req.Name, req.Price);
            req.Modifiers.ForEach(x => meal.AddModifier(x.Name, x.PriceAdjustment));
            restaurant.AddMeal(meal);
            
            await db.SaveChangesAsync(ct);
            
            await publisher.PublishEventAsync("pubsub", "restaurants-events",
                new RestaurantModifiedMessage(restaurantId), ct);

            return TypedResults.Ok(new CreateMealResponse(meal.Id, restaurant.Id));
        })
        .WithName("CreateRestaurantMeal")
        .WithSummary("Create Restaurant Meal")
        .WithTags("Meals")
        .Produces<CreateMealResponse>()
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }
}