using Domain.Abstractions;

namespace Domain.Tests.SampleDomain;

internal sealed class Parent(int id, string name, DateOnly dateOfBirth) : Person(id, name, dateOfBirth)
{
    public Option<string> Occupation { get; set; } = Option<string>.None;

    public IEnumerable<Child> Children { get; set; } = [];
}
