using Domain.Abstractions;

namespace Orders.Api.Domain;

public interface IOrderItem : IEntity<Guid>
{
    public Guid? ChildId { get; }

    public int Quantity { get; }

    public decimal TotalPrice { get; }
}