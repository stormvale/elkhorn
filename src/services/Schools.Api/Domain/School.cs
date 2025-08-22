using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Common;
using Domain.Results;
using ServiceDefaults.EfCore.Interfaces;
using ServiceDefaults.Middleware.MultiTenancy;

namespace Schools.Api.Domain;

public class School : AggregateRoot, ITenantAware, IAuditable
{
    [JsonConstructor] private School(Guid id) : base(id) { /* ef constructor */ }
    
    public static Result<School> Create(Guid id, string name, string externalId, Address address, Contact contact)
    {
        var school = new School(id)
        {
            Name = name,
            ExternalId = externalId,
            Address = address,
            Contact = contact,
            Pac = new Pac(Guid.CreateVersion7(), contact)
        };

        return Result.Success(school);
    }

    public Guid TenantId { get; set; }
    public string Name { get; private set; }
    public string ExternalId { get; private set; }
    public Address Address { get; private set; }
    public Contact Contact { get; private set; }
    public Pac Pac { get; private set; }
    
    #region IAuditable

    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? LastModifiedUtc { get; init; }

    #endregion
}