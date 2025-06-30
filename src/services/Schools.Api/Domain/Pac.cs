using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Common;

namespace Schools.Api.Domain;

public sealed class Pac : Entity
{
    [JsonConstructor] private Pac() : base(id: Guid.Empty) { /* ef constructor */ }
    
    public Pac(Guid id, Contact contact) : base(id)
    {
        Chairperson = contact;
    }

    public Contact Chairperson { get; set; }
    public List<LunchItem> LunchItems { get; set; } = [];
    
    public void AddLunchItem(string name, decimal price)
    {
        if (LunchItems.Any(x => x.Name == name))
        {
            throw new InvalidOperationException($"Lunch item with name {name} already exists.");
        }
        
        LunchItems.Add(new LunchItem(name, price));
    }

    public void RemoveLunchItem(string name) => LunchItems.RemoveAll(x => x.Name == name);
}

public record LunchItem(string Name, decimal Price);