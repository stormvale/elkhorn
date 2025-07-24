using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Interfaces;
using Domain.Results;
using Users.Api.DomainErrors;

namespace Users.Api.Domain;

/// <summary>
/// The User's ID comes from the http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier claim, which is
/// a stable, system-level ID for the user provided by Entra ID after authentication.
/// </summary>
public class User : AggregateRoot<string>, IAuditable
{
    [JsonConstructor] private User(string id) : base(id) { /* ef constructor */ }
    
    public static Result<User> Create(string id, string name, string email)
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
            Children.Add(child);
        }
    }
    
    #region IAuditable

    public DateTimeOffset CreatedUtc { get; init; }
    public DateTimeOffset? LastModifiedUtc { get; init; }

    #endregion
}