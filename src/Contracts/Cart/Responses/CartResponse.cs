namespace Contracts.Cart.Responses;

public record CartResponse(Guid Id, List<CartItemResponse> Items);