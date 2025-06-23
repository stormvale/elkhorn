using Domain.Abstractions;
using Lunches.Api.Domain.Events;

namespace Lunches.Api.Domain;

public class Lunch : AggregateRoot
{
    public Lunch(LunchScheduled @event) : base(@event.LunchId)
    {
        SchoolId = @event.SchoolId;
        RestaurantId = @event.RestaurantId;
        Date = @event.Date;
    }

    public DateOnly Date { get; init; }
    public Guid SchoolId { get; init; }
    public Guid RestaurantId { get; init; }
    public List<LunchItem> AvailablePacItems { get; init; } = [];
    
    public void Apply(ItemsAddedToLunch @event) => AvailablePacItems.AddRange(@event.Items);
}