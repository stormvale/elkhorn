using System.ComponentModel.DataAnnotations;

namespace Domain.Common;

public record Address(

    [Required]
    string Street,
    
    [Required]
    string City,
    
    [Required]
    string PostCode,

    [Required]
    string State
)
{
    public override string ToString() => $"{Street}, {City}, {PostCode}, {State}";
}
