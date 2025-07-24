using System.ComponentModel.DataAnnotations;
using Contracts.Common;

namespace Contracts.Users.Requests;

public record RegisterUserRequest(

    [Required]
    string Id,
    
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
    string Name,

    [Required]
    [EmailAddress]
    string Email);
