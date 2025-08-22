namespace Contracts.Cart.Responses;

public record CartItemResponse(
    Guid Id,
    string ItemType,
    Guid ItemId,
    string ItemName,
    int Quantity,
    string PayloadJson);