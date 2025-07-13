using Domain.Abstractions;
using Domain.Tests.SampleDomain;
using Shouldly;

namespace Domain.Tests.Abstractions;

public class OptionTests
{
    [Fact]
    public void Option_WhenValueNotSet_ShouldBeNone()
    {
        var p1 = new Parent(1, "John", new DateOnly(1980, 1, 1));
        
        p1.Occupation.HasValue.ShouldBeFalse();
        p1.Occupation.ShouldBe(Option<string>.None);
    }
    
    [Fact]
    public void Option_WhenValueNotSet_ValueOrDefaultShouldReturnDefault()
    {
        var p1 = new Parent(1, "John", new DateOnly(1980, 1, 1));
        
        p1.Occupation.ShouldBe(Option<string>.None);
        p1.Occupation.ValueOrDefault("Teacher").ShouldBe("Teacher");
    }
    
    [Fact]
    public void Option_WhenValueSet_ShouldBeValue()
    {
        var p1 = new Parent(1, "John", new DateOnly(1980, 1, 1))
        {
            // implicit conversion...
            Occupation = "Teacher"
        };

        p1.Occupation.HasValue.ShouldBeTrue();
        p1.Occupation.ShouldBe("Teacher");
    }
    
    [Fact]
    public void Option_WhenValueSet_ValueOrDefaultShouldReturnValue()
    {
        var p1 = new Parent(1, "John", new DateOnly(1980, 1, 1))
        {
            // implicit conversion...
            Occupation = "Teacher"
        };

        p1.Occupation.ValueOrDefault("sjdfhgsdjf").ShouldBe("Teacher");
    }
}
