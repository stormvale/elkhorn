using System.Text.Json.Nodes;
using Domain.Abstractions;

namespace Cart.Api.Domain;

public class CartItem : Entity<Guid>
{
    public CartItem(string type, string name, JsonObject payload, Guid? childId = null)
        : base(Guid.CreateVersion7())
    {
        Type = type;
        Name = name;
        Payload = payload;
        ChildId = childId;
    }

    public string Name { get; }
    public string Type { get; } // "Lunch", "Ticket", etc.
    public JsonObject Payload { get; }
    public Guid? ChildId { get; }
}