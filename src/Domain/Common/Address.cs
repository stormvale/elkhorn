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
    public static readonly Address Unknown = new(string.Empty, string.Empty, string.Empty, string.Empty);
    public override string ToString() => $"{Street}, {City}, {PostCode}, {State}";
}
