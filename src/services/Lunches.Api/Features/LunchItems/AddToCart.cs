using System.Text.Json;
using Contracts.Cart.Messages;
using Domain.Results;
using Lunches.Api.Domain;
using Lunches.Api.DomainErrors;
using Lunches.Api.EfCore;
using Lunches.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceDefaults.Middleware;
using ServiceDefaults.Middleware.MultiTenancy;

namespace Lunches.Api.Features.LunchItems;

public static class AddLunchItemToCart
{
    public static void MapAddLunchItemToCart(this RouteGroupBuilder group)
    {
        group.MapPost("/{lunchItemId:Guid}/add-to-cart", async (
            Guid lunchId,
            Guid lunchItemId,
            AppDbContext db,
            IRequestContextAccessor requestContext,
            ITenantAwarePublisher publisher,
            CancellationToken ct) =>
        {
            // load the lunch from the db
            var lunch = await db.Lunches.AsNoTracking()
                .Where(x => x.Id == lunchId)
                .FirstOrDefaultAsync(ct);
            
            if (lunch is null)
            {
                return Result.Failure(LunchErrors.NotFound(lunchId)).ToProblemDetails();
            }
        
            // find the lunch item
            var lunchItem = lunch.AvailableRestaurantItems
                .Concat(lunch.AvailablePacItems)
                .FirstOrDefault(x => x.Id == lunchItemId);
            
            if (lunchItem is null)
            {
                return Result.Failure(LunchItemErrors.NotFound(lunchItemId, lunchId)).ToProblemDetails();
            }

            var message = CreateAddItemToCartMessage(requestContext, lunchItem);
            await publisher.PublishEventAsync("pubsub", "cart-events", message, ct);

            return TypedResults.Ok();
        })
        .WithName("AddLunchItemToCart")
        .WithSummary("Add LunchItem To Cart")
        .WithTags("Lunches")
        .Produces(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }
    
    // create the message (user id is used as the cart id)
    private static AddItemToCartMessage CreateAddItemToCartMessage(IRequestContextAccessor requestContext, LunchItem lunchItem) => new (
            requestContext.Current.User.UserId,
            nameof(LunchItem),
            lunchItem.Name,
            JsonSerializer.Serialize(lunchItem.ToLunchItemResponse()));
}