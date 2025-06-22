using System.ComponentModel.DataAnnotations;

namespace Contracts.Lunches.Responses;

public class LunchItemResponse(string name, decimal price)
{
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
    public string Name { get; set; } = name;

    [Required]
    [Range(0, 10, ErrorMessage = "Price must be between 0 and 10.")]
    public decimal Price { get; set; } = price;
}
