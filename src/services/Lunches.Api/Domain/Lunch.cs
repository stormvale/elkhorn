using Domain.Abstractions;

namespace Lunches.Api.Domain;

public class Lunch : AggregateRoot
{
    // ef core constructor?
    // saga? (Scheduled, OpenForOrders, OrderingClosed, Delivered, Complete, Canceled)
    
    public Lunch(Guid id, Guid schoolId, Guid restaurantId, DateOnly date) : base(id)
    {
        SchoolId = schoolId;
        RestaurantId = restaurantId;
        Date = date;
    }

    public DateOnly Date { get; private set; }
    public Guid SchoolId { get; private set; }
    public Guid RestaurantId { get; private set; }
    public List<LunchItem> AvailablePacItems { get; init; } = [];
    public List<LunchItem> AvailableRestaurantItems { get; init; } = [];

    public void AddPacItems(IEnumerable<LunchItem> items) => AvailablePacItems.AddRange(items);
    public void AddRestaurantItems(IEnumerable<LunchItem> items) => AvailableRestaurantItems.AddRange(items);
}