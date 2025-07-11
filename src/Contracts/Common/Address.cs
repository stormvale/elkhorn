using System.ComponentModel.DataAnnotations;

namespace Contracts.Common;

public class Address(string street, string city, string postCode, string state)
{
    [Required]
    [StringLength(50, MinimumLength = 10, ErrorMessage = "Street length must be between 10 and 50 characters.")]
    public string Street { get; set; } = street;

    [Required]
    public string City { get; set; } = city;

    [Required]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Post Code must be 6 characters.")]
    public string PostCode { get; set; } = postCode;

    [Required]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "State must be 2 characters.")]
    public string State { get; set; } = state;

    public static Address Empty => new(string.Empty, string.Empty, string.Empty, string.Empty);

    public override string ToString() => $"{Street}, {City}, {PostCode}, {State}";
}
