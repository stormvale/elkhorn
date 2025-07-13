using System.ComponentModel.DataAnnotations;

namespace Contracts.Schools.Pac.Messages;

public record PacLunchItemRemoved(

    Guid SchoolId,
    
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
    string Name);
