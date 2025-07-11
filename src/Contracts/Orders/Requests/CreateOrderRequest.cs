using System.ComponentModel.DataAnnotations;
using Contracts.Common;

namespace Contracts.Orders.Requests;

public record CreateOrderRequest(

    [Required]
    Guid LunchId,
        
    [Required]
    Contact Contact
);