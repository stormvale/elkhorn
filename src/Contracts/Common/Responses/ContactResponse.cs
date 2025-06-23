using System.ComponentModel.DataAnnotations;

namespace Contracts.Common.Responses;

public class ContactResponse(string name, string email, string? phone, string type)
{
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
    public string Name { get; set; } = name;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = email;

    [Phone(ErrorMessage = "phone number is invalid")]
    public string? Phone { get; set; } = phone;

    public string Type { get; set; } = type;
    
    public static ContactResponse Empty => new(string.Empty, string.Empty, string.Empty, string.Empty);

    public override string ToString() => $"{Name} ({Email}) : {Type}";
}
