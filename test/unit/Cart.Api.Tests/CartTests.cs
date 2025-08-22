using Cart.Api.Domain;
using Contracts.Cart.Messages;
using Contracts.Lunches.Responses;
using Shouldly;

namespace Cart.Api.Tests;

public class UnitTest1
{
    [Fact]
    public void NewCart_ShouldCreateCorrectly()
    {
        var cart = new Domain.Cart(Guid.CreateVersion7(), []);
        
        cart.Items.ShouldBeEmpty();
    }
    
    [Fact]
    public void EmptyCart_AddLunchItem_ShouldAddCorrectly()
    {
        var userId = Guid.CreateVersion7();
        
        var message = AddItemToCartMessage.CreateFrom(userId, new LunchItemResponse(
            Guid.CreateVersion7(),
            "Test Lunch Item",
            5.50M,
            [new LunchItemModifierResponse("Test Modifier", 0.50M)]
        ));
        var cartItem = CartItem.CreateFrom(message);
        
        var cart = new Domain.Cart(userId, []);
        cart.AddItem(cartItem);
        cart.Items.Count.ShouldBe(1);
        
        cart.Items.First().ItemId.ShouldBe(cartItem.ItemId);
        cart.Items.First().ItemName.ShouldBe(cartItem.ItemName);
        cart.Items.First().ItemType.ShouldBe(cartItem.ItemType);
        cart.Items.First().Quantity.ShouldBe(1);
        
        // add another of the same item => should increase qty
        cart.AddItem(cartItem);
        cart.Items.Count.ShouldBe(1);
        cart.Items.First().Quantity.ShouldBe(2);
    }
}