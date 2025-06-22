using System.ComponentModel;
using Domain.Abstractions;

namespace Domain.Common;

public sealed class Contact : ValueObject
{
    public Contact() { /* ef constructor */ }
    
    public Contact(string name, string email, string? phone, ContactType contactType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        Name = name;
        Email = email;
        Phone = phone;
        Type = contactType;
    }

    // these property setters are important for ef to map the complex type
    
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public ContactType Type { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Email;
        yield return Phone ?? string.Empty;
        yield return Type;
    }

    public override string ToString() => $"{Name} ({Type})";
}

public enum ContactType
{
    [Description("Unknown")]
    Unknown,
    
    [Description("Manager")]
    Manager,
    
    [Description("Parent")]
    Parent,

    [Description("Principal")]
    Principal
}
