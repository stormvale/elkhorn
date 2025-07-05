using Domain.Abstractions;

namespace Orders.Api.Domain;

public class LunchOrderItem(Guid id, Guid childId, Guid lunchItemId, string description, decimal price) : Entity<Guid>(id)
{
    public Guid ChildId { get; init; } = childId;

    public Guid LunchItemId { get; } = lunchItemId;
    
    public string LunchItemDescription { get; private set; } = description;

    public decimal Price { get; private set; } = price;

}