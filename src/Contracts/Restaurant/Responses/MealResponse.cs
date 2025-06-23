using Contracts.Lunches.Responses;

namespace Contracts.Restaurant.Responses;

public class MealResponse(Guid id, string name, decimal price, List<MealModifierResponse> availableModifiers) : LunchItemResponse(name, price)
{
    public Guid Id { get; init; } = id;
    public List<MealModifierResponse> AvailableModifiers { get; init; } = availableModifiers;
    public static MealResponse Empty => new MealResponse(Guid.Empty, string.Empty, 0, []);
}
