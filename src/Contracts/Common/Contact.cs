using System.ComponentModel.DataAnnotations;

namespace Contracts.Common;

public class Contact(string name, string email, string? phone, ContactType type)
{
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
    public string Name { get; set; } = name;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = email;

    [Phone(ErrorMessage = "phone number is invalid")]
    public string? Phone { get; set; } = phone;

    public ContactType Type { get; set; } = type;
    
    public override string ToString() => $"{Name} ({Email}) : {Type}";
}

public enum ContactType
{
    Contact,
    Parent,
    Manager,
    Principal
}