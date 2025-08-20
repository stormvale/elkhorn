using System.Text.Json.Serialization;
using Domain.Abstractions;

namespace Cart.Api.Domain;

[method: JsonConstructor]
public class Cart(Guid id, List<CartItem>? items) : AggregateRoot(id)
{
    public List<CartItem> Items { get; } = items ?? [];

    public void AddItem(CartItem item) => Items.Add(item);
}