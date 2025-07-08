using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Common;

public record Contact(

    [Required]
    [StringLength(100, MinimumLength = 3)]
    string Name,

    [Required]
    [EmailAddress]
    string Email,

    [Phone]
    string? Phone,

    [Required]
    ContactType Type = ContactType.Unknown
)
{
    public override string ToString() => $"{Name} ({Type})";
}

public enum ContactType
{
    [Description("Unknown")] Unknown,
    [Description("Manager")] Manager,
    [Description("Parent")] Parent,
    [Description("Principal")] Principal
}
