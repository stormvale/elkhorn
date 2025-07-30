using System.ComponentModel.DataAnnotations;

namespace Contracts.Users.Requests;

public record RegisterUserRequest(

    [Required]
    Guid Id,
    
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
    string Name,

    [Required]
    [EmailAddress]
    string Email);
