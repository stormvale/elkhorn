﻿using Contracts.Users.Responses;
using Domain.Common;
using Users.Api.Domain;

namespace Users.Api.Extensions;

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
    
    public static UserResponse ToUserResponse(this User domainObject) => new(
        domainObject.Id,
        domainObject.Name,
        domainObject.Email,
        domainObject.SchoolIds.ToArray(),
        domainObject.Version
    );
}
