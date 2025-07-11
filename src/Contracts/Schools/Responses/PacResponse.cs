using Contracts.Common;
using Contracts.Lunches.Responses;

namespace Contracts.Schools.Responses;

public class PacResponse(Guid id, Contact chairperson, List<LunchItemResponse> lunchItems)
{
    public Guid Id { get; init; } = id;
    public Contact Chairperson { get; init; } = chairperson;
    public List<LunchItemResponse> LunchItems { get; init; } = lunchItems;
}
