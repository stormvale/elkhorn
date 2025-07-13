using Domain.Abstractions;

namespace Domain.Tests.SampleDomain;

internal abstract class Person(int id, string name, DateOnly dateOfBirth) : Entity<int>(id)
{
    public string Name { get; } = name;
    public DateOnly DateOfBirth { get; } = dateOfBirth;
    public int Age => DateTime.Now.Year - DateOfBirth.Year;
}
