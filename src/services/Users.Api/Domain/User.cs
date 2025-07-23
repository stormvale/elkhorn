using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Common;
using Domain.Interfaces;
using Domain.Results;

namespace Users.Api.Domain;

/// <summary>
/// The User's Id comes from the http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifie claim, which is
/// a stable, system-level ID for the user provided by Entra ID after authentication.
/// </summary>
public class User : AggregateRoot<string>, IAuditable
{
    [JsonConstructor] private User(string id) : base(id) { /* ef constructor */ }
    
    public static Result<User> Create(string id, string name, Address address)
    {
        var school = new User(id)
        {
            Name = name,
            Address = address,
        };

        return Result.Success(school);
    }

    public string Name { get; private set; }
    public Address Address { get; private set; }

    // EF Core Cosmos DB provider has a problem with List<Guid>
    public List<string> SchoolIds { get; set; }

    public List<Child> Children { get; private set; } = [];
    
    public void RegisterChild(string firstName, string lastName, Guid schoolId)
    {
        var child = Child.Create(Guid.CreateVersion7(), firstName, lastName, Id, schoolId);

        if (child.IsSuccess)
        {
            Children.Add(child);
        }
    }
    
    #region IAuditable

    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? LastModifiedUtc { get; init; }

    #endregion
}