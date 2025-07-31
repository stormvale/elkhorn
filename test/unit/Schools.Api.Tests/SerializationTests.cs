using Domain.Common;
using Schools.Api.Domain;
using Schools.Api.Extensions;
using Shouldly;

namespace Schools.Api.Tests;

public class SerializationTests
{
    [Fact]
    public void NewSchool_ShouldCreateCorrectly()
    {
        var address = new Address("Street", "City", "PostCode", "State");
        var contact = new Contact("Name", "Email", "Phone", ContactType.Principal);
        var school = School.Create(Guid.CreateVersion7(),"Test School", "123", address, contact).Value!;

        var response = school.ToSchoolResponse();
        response.ShouldNotBeNull();
        response.Contact.Type.ShouldBe(Contracts.Common.ContactType.Principal);
    }
}