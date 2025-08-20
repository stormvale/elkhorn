namespace Contracts.Lunches.Responses;

public record LunchItemResponse(
    Guid Id,
    string Name,
    decimal Price,
    List<LunchItemModifierResponse> AvailableModifiers
);