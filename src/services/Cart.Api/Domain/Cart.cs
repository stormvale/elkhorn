using System.Text.Json.Serialization;
using Domain.Abstractions;

namespace Cart.Api.Domain;

[method: JsonConstructor]
public class Cart(Guid id, List<CartItem>? items) : AggregateRoot(id)
{
    public List<CartItem> Items { get; } = items ?? [];

    public void AddItem(CartItem item)
    {
        var existingItem = Items.Find(x => x.ItemId == item.ItemId);
        if (existingItem is not null)
        {
            existingItem.Quantity++;
            return;
        }
    
        Items.Add(item);
    }
}