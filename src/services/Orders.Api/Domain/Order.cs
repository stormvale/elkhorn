using Domain.Abstractions;
using Domain.Common;
using ServiceDefaults.MultiTenancy;

namespace Orders.Api.Domain;

// saga? dapr workflow? (Placed, PaymentReceived, Complete, Canceled, ...?)
public class Order : AggregateRoot, ITenantAware
{
    public Order(Guid id, Guid lunchId, Contact parent) : base(id)
    {
        LunchId = lunchId;
        Parent = parent;
    }

    public Guid TenantId { get; set; }
    public Guid LunchId { get; private set; }
    public Contact Parent { get; private set; }
    public List<LunchOrderItem> Items { get; private set; } = [];

    public void AddItem(LunchOrderItem item) => Items.Add(item);
    
    public decimal GetTotal() => Items.Sum(i => i.Price);
}