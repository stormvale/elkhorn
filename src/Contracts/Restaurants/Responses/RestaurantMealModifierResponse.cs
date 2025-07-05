using System.ComponentModel.DataAnnotations;

namespace Contracts.Restaurants.Responses;

public class RestaurantMealModifierResponse(string name, decimal priceAdjustment)
{
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
    public string Name { get; set; } = name;

    [Required]
    [Range(-5, 5, ErrorMessage = "PriceAdjustment must be between -5 and 5.")]
    public decimal PriceAdjustment { get; set; } = priceAdjustment;
}
