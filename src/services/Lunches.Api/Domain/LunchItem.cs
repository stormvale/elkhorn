namespace Lunches.Api.Domain;

public record LunchItem(string Name, decimal Price, List<LunchItemModifier> AvailableModifiers)
{
    public LunchItem() : this(string.Empty, 0, [])
    {
        // ef constructor. required because of something to do with constructor parameter matching.
    }
}
