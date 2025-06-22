using Contracts.Common.Responses;

namespace Contracts.Schools.Responses;

public sealed record SchoolResponse(
    Guid Id,
    string Name,
    ContactResponse Contact,
    AddressResponse Address,
    PacResponse Pac,
    uint Version);
