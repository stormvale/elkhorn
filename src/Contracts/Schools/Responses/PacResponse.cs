using Contracts.Common;
using Contracts.Lunches.Responses;

namespace Contracts.Schools.Responses;

public record PacResponse(
    Guid Id,
    Contact Chairperson,
    List<LunchItemResponse> LunchItems);