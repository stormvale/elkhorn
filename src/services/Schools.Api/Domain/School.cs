using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Common;

namespace Schools.Api.Domain;

public class School : AggregateRoot, IAuditable
{
    [JsonConstructor] private School() : base(id: Guid.Empty) { /* ef constructor */ }
    
    public School(Guid id, string name, Address address, Contact contact) : base(id)
    {
        Name = name;
        Address = address;
        Contact = contact;
        Pac = new Pac(Guid.CreateVersion7(), contact);
    }

    public string Name { get; set; }
    public Address Address { get; set; }
    public Contact Contact { get; set; }
    public Pac Pac { get; set; }

    public void UpdateContact(Contact contact) => Contact = contact;
    
    #region IAuditable

    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? LastModifiedUtc { get; init; }

    #endregion
}