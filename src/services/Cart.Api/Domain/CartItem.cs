using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Contracts.Cart.Messages;
using Domain.Abstractions;

namespace Cart.Api.Domain;

public class CartItem : Entity<Guid>
{
    [JsonConstructor]
    private CartItem(Guid id, string itemType, Guid itemId, string itemName, int quantity, string payloadJson) : base(id)
    {
        ItemType = itemType;
        ItemId = itemId;
        ItemName = itemName;
        Quantity = quantity;
        PayloadJson = payloadJson;
    }
    
    public static CartItem CreateFrom(AddItemToCartMessage message)
    {
        return new CartItem(
            Guid.CreateVersion7(), 
            message.ItemType,
            message.ItemId,
            message.ItemName,
            quantity: 1,
            message.PayloadJson);
    }

    public string ItemType { get; private set; }
    public Guid ItemId { get; private set; }
    public string ItemName { get; private set; }
    public int Quantity { get; set; }
    public string PayloadJson { get; private set; }
    public Guid? ChildId { get; private set; }
    
    [JsonIgnore]
    public JsonObject PayloadObject => JsonSerializer.Deserialize<JsonObject>(PayloadJson)!;
}