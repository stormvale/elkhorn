using System.ComponentModel.DataAnnotations;

namespace Contracts.Users.DTOs;

// this will probably change to a more fully-featured response
public record ChildDto(
    Guid Id,
    
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
    string Name,
    
    string Grade);