using System.ComponentModel.DataAnnotations;

namespace Contracts.Users.DTOs;

public record UserSchoolDto(
    string Id,
    
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
    string Name,
    
    ChildDto[] Children);