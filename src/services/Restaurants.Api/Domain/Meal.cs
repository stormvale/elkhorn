using Domain.Abstractions;

namespace Restaurants.Api.Domain;

public class Meal(Guid id, string name, decimal price) : Entity(id)
{
    public string Name { get; set; } = name;
    public decimal Price { get; set; } = price;
    public List<MealModifier> AvailableModifiers { get; set; } = [];
    
    public void AddModifier(string name, decimal priceAdjustment) =>
        AvailableModifiers.Add(new MealModifier(name, priceAdjustment));

    public void RemoveModifier(string name) =>
        AvailableModifiers.RemoveAll(m => m.Name == name);
}

public record MealModifier(string Name, decimal PriceAdjustment);
