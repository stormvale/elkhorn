using System.Text.Json;
using System.Text.Json.Nodes;
using ServiceDefaults.Middleware.MultiTenancy;

namespace Contracts.Cart.Messages;

/// <summary>
/// Represents a message used to add an item to a cart.
/// </summary>
/// <param name="CartId">The unique identifier for the cart. (This is the User Id.)</param>
/// <param name="ItemType">Type of the item being added.</param>
/// <param name="ItemName">Name of the item being added.</param>
/// <param name="PayloadJson">JSON payload containing additional details about the item.</param>
public record AddItemToCartMessage(
    Guid CartId,
    string ItemType,
    string ItemName,
    string PayloadJson) : ITenantAware
{
    public Guid TenantId { get; set; }
    
    public JsonObject? PayloadObject => JsonSerializer.Deserialize<JsonObject>(PayloadJson);
}