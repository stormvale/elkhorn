using Contracts.Common.Responses;
using Contracts.Orders.Responses;
using Domain.Common;
using Orders.Api.Domain;

namespace Orders.Api.Extensions;

public static class MappingExtensions
{
    public static OrderResponse ToOrderResponse(this Order order) => 
        new(order.Id);
    
    public static Contact ToContact(this ContactResponse dto) => 
        new(dto.Name, dto.Email, dto.Phone, Enum.Parse<ContactType>(dto.Type));
}
