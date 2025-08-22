using Domain.Abstractions;
using Domain.Interfaces;
using Domain.Results;

namespace Lunches.Api.Domain;

public class LunchItem(Guid id) : Entity(id), IPurchasable
{
    // ef constructor?
    
    public static Result<LunchItem> Create(string name, decimal price, List<LunchItemModifier> availableModifiers)
    {
        var itemId = Guid.CreateVersion7();
        
        var item = new LunchItem(itemId)
        {
            Name = name,
            Price = price,
            AvailableModifiers = availableModifiers
        };

        return Result.Success(item);
    }

    public string Type => "LunchItem";
    public required string Name { get; init; }
    public decimal Price { get; init; }
    public List<LunchItemModifier> AvailableModifiers { get; init; } = [];
}