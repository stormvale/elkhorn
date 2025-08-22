using Cart.Api.Domain;
using Contracts.Cart.Responses;

namespace Cart.Api.Extensions;

public static class MappingExtensions
{
    public static CartResponse ToCartResponse(this Cart.Api.Domain.Cart cart)
    {
        return new CartResponse(
            cart.Id,
            cart.Items.Select(x => x.ToCartItemResponse()).ToList()
        );
    }

    public static CartItemResponse ToCartItemResponse(this CartItem item) => new(
        Id: item.Id,
        ItemType: item.ItemType,
        ItemId: item.ItemId,
        ItemName: item.ItemName,
        Quantity: item.Quantity,
        PayloadJson: item.PayloadJson
    );
}
