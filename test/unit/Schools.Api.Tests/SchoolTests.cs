using Domain.Common;
using Schools.Api.Domain;
using Shouldly;

namespace Schools.Api.Tests;

public class SchoolTests
{
    [Fact]
    public void NewSchool_ShouldCreateCorrectly()
    {
        var address = new Address("Street", "City", "PostCode", "State");
        var contact = new Contact("Name", "Email", "Phone", ContactType.Principal);
        School school = School.Create(Guid.CreateVersion7(),"Test School", "123", address, contact);
    
        // id generated only when persisted
        school.Id.ShouldNotBe(Guid.Empty);
        school.Name.ShouldBe("Test School");
        school.ExternalId.ShouldBe("123");
        school.Address.ShouldBe(address);
        school.Contact.ShouldBe(contact);
        school.Pac.ShouldNotBeNull();
        
        var pac = school.Pac;
        pac.Id.ShouldNotBe(Guid.Empty);
        pac.Chairperson.ShouldBe(school.Contact);
        pac.LunchItems.ShouldBeEmpty();
    }
}