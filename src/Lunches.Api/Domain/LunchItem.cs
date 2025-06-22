namespace Lunches.Api.Domain;

public record LunchItem(
    string Name,
    decimal Price,
    List<LunchItemModifier> AvailableModifiers);