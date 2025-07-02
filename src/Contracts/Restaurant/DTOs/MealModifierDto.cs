using System.ComponentModel.DataAnnotations;

namespace Contracts.Restaurant.DTOs;

public record MealModifierDto(

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
    string Name,

    [Required]
    [Range(-10, 10, ErrorMessage = "PriceAdjustment must be between -10 and 10.")]
    decimal PriceAdjustment
);
