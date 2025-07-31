using Domain.Results;
using Shouldly;
using Users.Api.Domain;
using Users.Api.DomainErrors;

namespace Users.Api.Tests;

public class UserTests
{
    [Fact]
    public void NewUser_ShouldCreateCorrectly()
    {
        var userCreateResult = User.Create(Guid.CreateVersion7(),"Bob Test", "bob@test.com");
        
        userCreateResult.IsSuccess.ShouldBeTrue();
        userCreateResult.Value.ShouldNotBeNull();
        userCreateResult.Error.ShouldBe(DomainError.None);
        
        var user = userCreateResult.Value;
        user.Id.ShouldNotBe(Guid.Empty);
        user.Name.ShouldBe("Bob Test");
        user.SchoolIds.ShouldBeEmpty();
    }
    
    [Fact]
    public void RegisterChild_SuccessWhenNotExists_FailIfExists()
    {
        var schoolId = Guid.NewGuid();
        var user = User.Create(Guid.CreateVersion7(),"Bob Test", "bob@test.com").Value!;
        var child = Child.Create("Billy", "Smith", user.Id, schoolId, "School of Test", "2nd grade");

        var registerResult1 = user.RegisterChild(child);

        registerResult1.ShouldBeEquivalentTo(Result.Success());
        user.Children.Count.ShouldBe(1);
        user.Children.ShouldContain(child);
        user.SchoolIds.Count.ShouldBe(1);
        user.SchoolIds.ShouldContain(schoolId);
            
        var registerResult2 = user.RegisterChild(child);
        registerResult2.ShouldBeEquivalentTo(
            Result.Failure(UserErrors.ChildAlreadyRegistered(user.Id, child.Value!.Name)));
        user.Children.Count.ShouldBe(1);
    }
}