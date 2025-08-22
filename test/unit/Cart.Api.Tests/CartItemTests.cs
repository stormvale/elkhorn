using Cart.Api.Domain;
using Contracts.Cart.Messages;
using Contracts.Lunches.Responses;
using Shouldly;

namespace Cart.Api.Tests;

public class CartItemTests
{
    [Fact]
    public void CartItem_CreateFromMessage_ShouldCreateCorrectly()
    {
        var userId = Guid.CreateVersion7();
        
        var lunchItemResponse = new LunchItemResponse(
            Guid.CreateVersion7(),
            "Test Lunch Item",
            5.50M,
            [new LunchItemModifierResponse("Test Modifier", 0.50M)]
        );
            
        var message = AddItemToCartMessage.CreateFrom(userId, lunchItemResponse);
        var cartItem = CartItem.CreateFrom(message);
        
        cartItem.ItemId.ShouldBe(lunchItemResponse.Id);
        cartItem.ItemName.ShouldBe(lunchItemResponse.Name);
        cartItem.ItemType.ShouldBe(typeof(LunchItemResponse).FullName);
        cartItem.Quantity.ShouldBe(1);
    }
}