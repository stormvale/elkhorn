using Domain.Abstractions;
using Domain.Interfaces;

namespace Orders.Api.Domain;

public interface IOrderItem : IEntity<Guid>, IPurchasable
{
    public Guid? ChildId { get; }

    public int Quantity { get; }

    public decimal TotalPrice { get; }
    
}