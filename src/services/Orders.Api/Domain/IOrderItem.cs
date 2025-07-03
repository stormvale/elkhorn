using Domain.Abstractions;

namespace Orders.Api.Domain;

public interface IOrderItem : IEntity<Guid>
{
    public Guid ChildId { get; }

    public decimal GetTotal();
}