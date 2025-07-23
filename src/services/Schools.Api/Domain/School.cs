using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Common;
using Domain.Interfaces;
using Domain.Results;

namespace Schools.Api.Domain;

public class School : AggregateRoot, IAuditable
{
    [JsonConstructor] private School(Guid id) : base(id) { /* ef constructor */ }
    
    public static Result<School> Create(Guid id, string name, Address address, Contact contact)
    {
        var school = new School(id)
        {
            Name = name,
            Address = address,
            Contact = contact,
            Pac = new Pac(Guid.CreateVersion7(), contact)
        };

        return Result.Success(school);
    }

    public string Name { get; private set; }
    public Address Address { get; private set; }
    public Contact Contact { get; private set; }
    public Pac Pac { get; private set; }

    public void UpdateContact(Contact contact) => Contact = contact;
    
    #region IAuditable

    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? LastModifiedUtc { get; init; }

    #endregion
}