using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Common;
using ServiceDefaults.MultiTenancy;

namespace Restaurants.Api.Domain;

public class Restaurant : AggregateRoot
{
    [JsonConstructor] private Restaurant() : base(id: Guid.Empty) { /* ef constructor */ }
    
    public Restaurant(Guid id, string name, Address address, Contact contact) : base(id)
    {
        Name = name;
        Address = address;
        Contact = contact;
    }
    
    public string Name { get; private set; }
    public Address Address { get; private set; }
    public Contact Contact { get; private set; }
    public ICollection<Meal> Menu { get; private set; } = [];
    
    public void AddMeal(Meal meal)
    {
        if (Menu.Any(m => m.Name == meal.Name))
        {
            throw new InvalidOperationException($"Meal {meal.Name} already exists");
        }
        
        Menu.Add(meal);
    }
    
    public void RemoveMeal(Guid mealId)
    {
        var meal = Menu.First(m => m.Id == mealId)
            ?? throw new InvalidOperationException($"Meal with Id {mealId} does not exist");
        
        Menu.Remove(meal);
    }
    
    public void UpdateMeal(Guid mealId, string name, decimal price, List<MealModifier> modifiers)
    {
        var existing = Menu.First(m => m.Id == mealId)
            ?? throw new InvalidOperationException($"Meal with Id {mealId} does not exist");
        
        existing.Name = name;
        existing.Price = price;
        existing.AvailableModifiers = modifiers;
    }
    
    public void UpdateAddress(Address address) => Address = address;

    public void UpdateContact(Contact contact) => Contact = contact;
}