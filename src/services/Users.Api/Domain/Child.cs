using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Interfaces;
using Domain.Results;

namespace Users.Api.Domain;

/// <summary>
/// This will probably move out into its own service. Just doing it here for now
/// </summary>
public class Child : AggregateRoot, IAuditable
{
    [JsonConstructor] private Child(Guid id) : base(id) { /* ef constructor */ }
    
    public static Result<Child> Create(Guid id, string firstName, string lastName, string parentId, Guid schoolId)
    {
        var child = new Child(id)
        {
            FirstName = firstName,
            LastName = lastName,
            ParentId = parentId,
            SchoolId = schoolId,
            CreatedUtc = DateTime.UtcNow
        };

        return Result.Success(child);
    }

    public string Name => $"{FirstName} {LastName}";
    
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string ParentId { get; private set; } // this is a UserId
    public string Grade { get; set; }
    public Guid SchoolId { get; private set; }
    
    #region IAuditable

    public DateTimeOffset CreatedUtc { get; private init; }
    public DateTimeOffset? LastModifiedUtc { get; set; }

    #endregion
}