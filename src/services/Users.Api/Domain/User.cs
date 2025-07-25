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
            Email = email,
            SchoolIds = []
        };

        return Result.Success(school);
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public List<string> SchoolIds { get; private set; } = []; // EF core can't do List<Guid> with Cosmos
    public List<Child> Children { get; } = [];

    public Result LinkSchool(Guid schoolId)
    {
        if (SchoolIds.Contains(schoolId.ToString()))
        {
            return Result.Failure(UserErrors.AlreadyLinkedToSchool(Id, schoolId));
        }
        
        SchoolIds.Add(schoolId.ToString());
        return Result.Success();
    }
    
    public void RegisterChild(string firstName, string lastName, Guid schoolId)
    {
        var child = Child.Create(Guid.CreateVersion7(), firstName, lastName, Id, schoolId);

        if (child.IsSuccess)
        {
            Children.Add(child.Value);
        }
    }
    
    #region IAuditable

    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? LastModifiedUtc { get; init; }

    #endregion
}