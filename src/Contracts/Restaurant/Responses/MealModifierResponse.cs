using System.ComponentModel.DataAnnotations;

namespace Contracts.Restaurant.Responses;

public class MealModifierResponse(string name, decimal priceAdjustment)
{
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
    public string Name { get; set; } = name;

    [Required]
    [Range(-10, 10, ErrorMessage = "PriceAdjustment must be between -10 and 10.")]
    public decimal PriceAdjustment { get; set; } = priceAdjustment;
}
