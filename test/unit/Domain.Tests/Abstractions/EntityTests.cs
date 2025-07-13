using Domain.Tests.SampleDomain;
using Shouldly;

namespace Domain.Tests.Abstractions;

public class EntityTests
{
    [Fact]
    public void Equality_SameIdsDifferentProperties_AreEqual()
    {
        var p1 = new Parent(1, "John", new DateOnly(1980, 1, 1));
        var p2 = new Parent(1, "Bill", new DateOnly(1981, 2, 3));

        p1.ShouldBe(p2);
    }
    
    [Fact]
    public void Equality_DifferentIdsSameProperties_AreNotEqual()
    {
        var p1 = new Parent(1, "John", new DateOnly(1980, 1, 1));
        var p2 = new Parent(2, "John", new DateOnly(1980, 1, 1));

        p1.ShouldNotBe(p2);
    }
}
