namespace Domain.Tests.SampleDomain;

internal sealed class Child(int id, string name, DateOnly dateOfBirth) : Person(id, name, dateOfBirth)
{
    public string School { get; set; } = string.Empty;
}
