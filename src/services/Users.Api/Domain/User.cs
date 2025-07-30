using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Interfaces;
using Domain.Results;
using Users.Api.DomainErrors;

namespace Users.Api.Domain;

/// <summary>
/// The User's ID comes from the 'oid' claim because it is the immutable ID
/// of the user in Entra ID across all apps in the tenant.
/// </summary>
public class User : AggregateRoot, IAuditable
{
    [JsonConstructor] private User(Guid id) : base(id) { /* ef constructor */ }
    
    public static Result<User> Create(Guid id, string name, string email)
    {
        var school = new User(id)
        {
            Name = name,
            Email = email
        };

        return Result.Success(school);
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public List<Child> Children { get; } = [];

    public List<Guid> SchoolIds => [.. Children.Select(c => c.SchoolId)];

    public Result RegisterChild(Child child)
    {
        if (Children.Any(x => x.Name == child.Name))
        {
            return Result.Failure(UserErrors.ChildAlreadyRegistered(Id, child.Name));
        }
        
        Children.Add(child);
        return Result.Success();
    }
    
    #region IAuditable

    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? LastModifiedUtc { get; init; }

    #endregion
}