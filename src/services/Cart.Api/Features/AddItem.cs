using Cart.Api.Domain;
using Contracts.Cart.Messages;
using ServiceDefaults.Middleware.MultiTenancy;

namespace Cart.Api.Features;

public static class AddItem
{
    public static void MapAddItem(this WebApplication app)
    {
        app.MapPost("/add-item-to-cart", async (
            AddItemToCartMessage message,
            ITenantAwareStateStore state,
            CancellationToken ct) =>
        {
            var cartId = message.CartId;

            var cart = await state.GetStateAsync<Domain.Cart>(cartId.ToString(), ct)
                       ?? new Domain.Cart(cartId, []);

            var cartItem = CartItem.CreateFrom(message);
            
            cart.AddItem(cartItem);
            
            await state.SaveStateAsync(cart.Id.ToString(), cart, ct);
            
            return TypedResults.Ok();
        })
        .WithName("AddItemToCart")
        .WithSummary("Add Item To Cart")
        .WithTags("Cart")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}