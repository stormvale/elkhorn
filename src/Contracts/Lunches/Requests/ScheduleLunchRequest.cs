using System.ComponentModel.DataAnnotations;

namespace Contracts.Lunches.Requests;

public record ScheduleLunchRequest(

    [Required]
    Guid SchoolId,

    [Required]
    Guid RestaurantId,

    [Required]
    [Range(typeof(DateOnly), "2025-07-01", "2030-12-31", ErrorMessage = "Lunch date must be in the future.")]
    DateOnly Date);