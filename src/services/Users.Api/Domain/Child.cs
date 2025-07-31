using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Interfaces;
using Domain.Results;

namespace Users.Api.Domain;

/// <summary>
/// This will probably move out into its own service. Just doing it here for now
/// </summary>
public sealed class Child : Entity
{
    [JsonConstructor] private Child(Guid id) : base(id) { /* ef constructor */ }
    
    public static Result<Child> Create(string firstName, string lastName, Guid parentId, Guid schoolId, string schoolName, string grade)
    {
        var child = new Child(Guid.CreateVersion7())
        {
            FirstName = firstName,
            LastName = lastName,
            ParentId = parentId,
            SchoolId = schoolId,
            SchoolName = schoolName, 
            Grade = grade
        };

        return Result.Success(child);
    }

    public string Name => $"{FirstName} {LastName}";
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    
    /// <summary>
    /// The UserID of the Parent/Guardian
    /// </summary>
    public Guid ParentId { get; private set; }
    public string Grade { get; private set; }
    public Guid SchoolId { get; private set; }
    public string SchoolName { get; private set; }

    public void UpdatePersonalInfo(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public void UpdateSchoolInfo(Guid schoolId, string schoolName, string grade)
    {
        SchoolId = schoolId;
        SchoolName = schoolName;
        Grade = grade;
    }
}