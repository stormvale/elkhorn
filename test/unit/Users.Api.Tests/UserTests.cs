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
        var user = User.Create(Guid.CreateVersion7(),"Bob Test", "bob@test.com").Value!;
        
        user.Id.ShouldNotBe(Guid.Empty);
        user.Name.ShouldBe("Bob Test");
        user.SchoolIds.ShouldBeEmpty();
    }
    
    [Fact]
    public void LinkSchool_SuccessWhenNotExists_FailIfExists()
    {
        var schoolId = Guid.NewGuid();
        var user = User.Create(Guid.CreateVersion7(),"Bob Test", "bob@test.com").Value!;

        var result1 = user.LinkSchool(schoolId);
        result1.ShouldBeEquivalentTo(Result.Success());
        user.SchoolIds.Count.ShouldBe(1);
        user.SchoolIds.ShouldContain(schoolId.ToString());
            
        var result2 = user.LinkSchool(schoolId);
        result2.ShouldBeEquivalentTo(Result.Failure(UserErrors.AlreadyLinkedToSchool(user.Id, schoolId)));
        user.SchoolIds.Count.ShouldBe(1);
    }
}