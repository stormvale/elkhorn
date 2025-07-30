using System.ComponentModel.DataAnnotations;

namespace Contracts.Users.Requests;

/// <summary>
/// The User ID (Parent) will come from the route. 
/// </summary>
/// <param name="FirstName">First Name of the Child.</param>
/// <param name="LastName">Last Name of the Child.</param>
/// <param name="SchoolId">ID of the School which the Child attends.</param>
/// <param name="SchoolName">Name of the School which the Child attends.</param>
/// <param name="Grade">Grade that the Child is in.</param>
public record RegisterChildRequest(
    
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "FirstName must be between 3 and 50 characters.")]
    string FirstName,
    
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "LastName must be between 3 and 50 characters.")]
    string LastName,
    
    [Required]
    Guid SchoolId,
    
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
    string SchoolName,

    [Required]
    string Grade);
