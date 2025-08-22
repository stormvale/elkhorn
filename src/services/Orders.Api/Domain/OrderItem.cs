using Domain.Abstractions;
using Domain.Interfaces;
using Domain.Results;

namespace Orders.Api.Domain;

public class OrderItem<T>(Guid id) : Entity(id), IOrderItem where T : Entity, IPurchasable
{
    // ef constructor?
    
    public static Result<OrderItem<T>> Create(T item, int quantity)
    {
        var orderItemId = Guid.CreateVersion7();
        
        var orderItem = new OrderItem<T>(orderItemId)
        {
            Quantity = quantity,
            Price = item.Price
        };

        return Result.Success(orderItem);
    }
    
    public Guid? ChildId { get; }
    
    public decimal Price { get; init; }
    public int Quantity { get; init; }
    
    public decimal TotalPrice => Price * Quantity;
}