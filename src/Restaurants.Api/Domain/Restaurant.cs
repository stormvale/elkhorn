using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Common;
using Restaurants.Api.Domain.Events;

namespace Restaurants.Api.Domain;

public class Restaurant : AggregateRoot, IHasDomainEvents, IAuditable
{
    [JsonConstructor] private Restaurant() : base(id: Guid.Empty) { /* ef constructor */ }
    
    public Restaurant(Guid id, string name, Address address, Contact contact) : base(id)
    {
        Name = name;
        Address = address;
        Contact = contact;
        
        AddDomainEvent(new RestaurantRegisteredDomainEvent(id));
    }

    public string Name { get; } = string.Empty;
    public Address Address { get; }
    public Contact Contact { get; private set; }
    public ICollection<Meal> Menu { get; } = [];
    
    public void AddMeal(Meal meal)
    {
        if (Menu.Any(m => m.Name == meal.Name))
        {
            throw new InvalidOperationException($"Meal {meal.Name} already exists");
        }
        
        Menu.Add(meal);

        AddDomainEvent(new MealAddedDomainEvent(Id, meal.Id));
    }
    
    public void RemoveMeal(Guid mealId)
    {
        var meal = Menu.First(m => m.Id == mealId)
            ?? throw new InvalidOperationException($"Meal with Id {mealId} does not exist");
        
        Menu.Remove(meal);
        
        AddDomainEvent(new MealRemovedDomainEvent(Id, meal.Id));
    }
    
    public void UpdateMeal(Guid mealId, string name, decimal price, List<MealModifier> modifiers)
    {
        var existing = Menu.First(m => m.Id == mealId)
            ?? throw new InvalidOperationException($"Meal with Id {mealId} does not exist");
        
        existing.Name = name;
        existing.Price = price;
        existing.AvailableModifiers = modifiers;

        AddDomainEvent(new MealUpdatedDomainEvent(Id, existing.Id));
    }
    
    public void UpdateContact(Contact contact)
    {
        Contact = contact;
        
        AddDomainEvent(new RestaurantContactUpdatedDomainEvent(Id, contact.Name));
    }

    #region IHasDomainEvents

    [JsonIgnore]
    private readonly Queue<DomainEvent> _domainEvents = [];

    public void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Enqueue(domainEvent);

    public IReadOnlyList<DomainEvent> PopDomainEvents()
    {
        var copy = _domainEvents.ToArray();

        _domainEvents.Clear();

        return copy.AsReadOnly();
    }

    #endregion
    
    #region IAuditable

    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? LastModifiedUtc { get; init; }

    #endregion
}
