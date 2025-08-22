using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Contracts.Lunches.Responses;
using ServiceDefaults.Middleware.MultiTenancy;

namespace Contracts.Cart.Messages;

/// <summary>
/// Represents a message for adding an item to a cart.
/// </summary>
public record AddItemToCartMessage : ITenantAware
{
    public Guid CartId { get; }
    public string ItemType { get; }
    public Guid ItemId { get; }
    public string ItemName { get; }
    public string PayloadJson { get; }
    public Guid TenantId { get; set; }

    [JsonConstructor]
    private AddItemToCartMessage(Guid cartId, string itemType, Guid itemId, string itemName, string payloadJson)
    {
        CartId = cartId;
        ItemType = itemType;
        ItemId = itemId;
        ItemName = itemName;
        PayloadJson = payloadJson;
    }

    public static AddItemToCartMessage CreateFrom(Guid cartId, LunchItemResponse item)
    {
        return new AddItemToCartMessage(
            cartId,
            item.GetType().FullName!,
            item.Id,
            item.Name,
            JsonSerializer.Serialize(item)
        );
    }
}