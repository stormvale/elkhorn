using System.ComponentModel.DataAnnotations;
using Contracts.Common;

namespace Contracts.Schools.Requests;

public record RegisterSchoolRequest(

    [Required]
    string ExternalId,
    
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
    string Name,

    [Required]
    Address Address,

    [Required]
    Contact Contact);
