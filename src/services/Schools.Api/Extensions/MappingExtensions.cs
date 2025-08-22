using Contracts.Lunches.Responses;
using Contracts.Schools.Responses;
using Domain.Common;
using Schools.Api.Domain;

namespace Schools.Api.Extensions;

public static class MappingExtensions
{
    public static Contact ToDomainContact(this Contracts.Common.Contact dto) => 
        new(dto.Name, dto.Email, dto.Phone, (ContactType)dto.Type);

    public static Address ToDomainAddress(this Contracts.Common.Address dto) =>
        new(dto.Street, dto.City, dto.PostCode, dto.State);

    private static Contracts.Common.Contact ToResponse(this Contact contact) => 
        new(contact.Name, contact.Email, contact.Phone, (Contracts.Common.ContactType)contact.Type);

    private static Contracts.Common.Address ToResponse(this Address address) =>
        new(address.Street, address.City, address.PostCode, address.State);

    private static PacResponse ToResponse(this Pac pac) => new(
        pac.Id,
        pac.Chairperson.ToResponse(),
        pac.LunchItems.Select(x => new LunchItemResponse(x.Id, x.Name, x.Price, [])).ToList());

    public static SchoolResponse ToSchoolResponse(this School school) => new(
        school.Id,
        school.Name,
        school.ExternalId,
        school.Contact.ToResponse(),
        school.Address.ToResponse(),
        school.Pac.ToResponse()
    );
}
