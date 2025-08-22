using Cart.Api.DomainErrors;
using Cart.Api.Extensions;
using Contracts.Cart.Responses;
using Domain.Results;
using ServiceDefaults.Middleware;
using ServiceDefaults.Middleware.MultiTenancy;

namespace Cart.Api.Features;

public static class GetById
{
    public static void MapGetById(this WebApplication app)
    {
        app.MapGet("/{cartId:Guid}", async (
            Guid cartId,
            IRequestContextAccessor requestContext,
            ITenantAwareStateStore state,
            CancellationToken ct) =>
        {
            var tenantId = requestContext.Current.Tenant.TenantId;
            var cart = await state.GetStateAsync<Domain.Cart>(tenantId, cartId.ToString(), ct);

            return cart is null
                ? Result.Failure(CartErrors.NotFound(cartId)).ToProblemDetails()
                : TypedResults.Ok(cart.ToCartResponse());
        })
        .WithName("GetCartById")
        .WithSummary("Get Cart by Id")
        .WithTags("Cart")
        .Produces<CartResponse>()
        .Produces(StatusCodes.Status404NotFound);
    }
}