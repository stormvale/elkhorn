using System.ComponentModel.DataAnnotations;
using Contracts.Common.Responses;

namespace Contracts.Orders.Requests;

public record CreateOrderRequest(

    [Required]
    Guid LunchId,
        
    [Required]
    ContactResponse Contact
);