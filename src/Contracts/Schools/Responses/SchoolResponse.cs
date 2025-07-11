using Contracts.Common;

namespace Contracts.Schools.Responses;

public sealed record SchoolResponse(
    Guid Id,
    string Name,
    Contact Contact,
    Address Address,
    PacResponse Pac,
    uint Version);
