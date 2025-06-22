using Contracts.Common.Responses;
using Contracts.Lunches.Responses;

namespace Contracts.Schools.Responses;

public class PacResponse(Guid id, ContactResponse chairperson, List<LunchItemResponse> lunchItems)
{
    public Guid Id { get; init; } = id;
    public ContactResponse Chairperson { get; init; } = chairperson;
    public List<LunchItemResponse> LunchItems { get; init; } = lunchItems;
}
