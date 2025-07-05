using System.ComponentModel.DataAnnotations;
using Contracts.Restaurants.DTOs;

namespace Contracts.Restaurants.Requests;

public record CreateMealRequest(

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
    string Name,

    [Required]
    [Range(0, 10, ErrorMessage = "Price must be between 0 and 10")]
    decimal Price,

    List<MealModifierDto> Modifiers);
