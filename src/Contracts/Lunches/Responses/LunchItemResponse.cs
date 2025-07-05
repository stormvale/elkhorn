using System.ComponentModel.DataAnnotations;

namespace Contracts.Lunches.Responses;

public record LunchItemResponse(

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
    string Name,

    [Required]
    [Range(0, 10, ErrorMessage = "Price must be between 0 and 10.")]
    decimal Price,
    
    List<LunchItemModifierResponse> AvailableModifiers
);