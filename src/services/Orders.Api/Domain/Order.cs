using Domain.Abstractions;
using Domain.Common;

namespace Orders.Api.Domain;

// saga? (Placed, PaymentReceived, Complete, Canceled, ...?)
public class Order : AggregateRoot
{
    /// <summary>
    /// Currently, an Order is specifically for a Lunch. This may change in the future to include other things. 
    /// </summary>
    public Order(Guid id, Guid lunchId, Contact parent) : base(id)
    {
        LunchId = lunchId;
        Parent = parent;
    }

    public Guid LunchId { get; private set; }
    public Contact Parent { get; private set; }
    public List<LunchOrderItem> Items { get; private set; } = [];

    public void AddItem(LunchOrderItem item) => Items.Add(item);
    
    public decimal GetTotal() => Items.Sum(i => i.Price);
}