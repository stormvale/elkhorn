using Contracts.Common.Responses;
using Contracts.Lunches.Responses;
using Contracts.Schools.Responses;
using Domain.Common;
using Schools.Api.Domain;

namespace Schools.Api.Extensions;

public static class MappingExtensions
{
    public static Contact ToContact(this ContactResponse dto) => 
        new(dto.Name, dto.Email, dto.Phone, Enum.Parse<ContactType>(dto.Type));

    public static Address ToAddress(this AddressResponse addressDto) =>
        new(addressDto.Street, addressDto.City, addressDto.PostCode, addressDto.State);

    private static ContactResponse ToResponse(this Contact contact) => 
        new(contact.Name, contact.Email, contact.Phone, Enum.GetName(contact.Type)!);

    private static AddressResponse ToResponse(this Address address) =>
        new(address.Street, address.City, address.PostCode, address.State);
    
    private static PacResponse ToResponse(this Pac pac) => new(
        pac.Id,
        pac.Chairperson.ToResponse(),
        pac.LunchItems.Select(x => new LunchItemResponse(x.Name, x.Price)).ToList());
    
    public static SchoolResponse ToSchoolResponse(this School school) => new(
        school.Id,
        school.Name,
        school.Contact.ToResponse(),
        school.Address.ToResponse(),
        school.Pac.ToResponse(),
        school.Version
    );
}
