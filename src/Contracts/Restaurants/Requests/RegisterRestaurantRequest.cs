using System.ComponentModel.DataAnnotations;
using Contracts.Common.Responses;

namespace Contracts.Restaurants.Requests;

public record RegisterRestaurantRequest(

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
    string Name,

    [Required]
    AddressResponse Address,

    [Required]
    ContactResponse Contact);
