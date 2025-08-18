using Contracts.Restaurants.Responses;
using Contracts.Schools.Responses;
using Domain.Abstractions;
using Domain.Results;
using Lunches.Api.DomainErrors;
using Lunches.Api.Extensions;
using ServiceDefaults.MultiTenancy;

namespace Lunches.Api.Domain;

// saga? (Scheduled, OpenForOrders, OrderingClosed, Delivered, Complete, Canceled)
public class Lunch(Guid id) : AggregateRoot(id), ITenantAware
{
    // ef core constructor?
    
    // builder pattern?
    public static Result<Lunch> Create(SchoolResponse school, RestaurantResponse restaurant, DateOnly date)
    {
        var lunchId = Guid.CreateVersion7();

        var lunch = new Lunch(lunchId)
        {
            SchoolId = school.Id,
            RestaurantId = restaurant.Id,
            Date = date
        };
        
        // validate invariants...
        if (school.Pac.LunchItems.Count + restaurant.Menu.Count < 0)
        {
            return Result.Failure<Lunch>(LunchErrors.NoLunchItemsAvailable());
        }

        foreach (var item in restaurant.Menu)
        {
            LunchItem lunchItem = LunchItem.Create(item.Name, item.Price, item.AvailableModifiers.ToLunchItemModifiers());
            lunch.AddRestaurantItem(lunchItem);
        }

        foreach (var item in school.Pac.LunchItems)
        {
            lunch.AddPacItem(LunchItem.Create(item.Name, item.Price, []));
        }
        
        return Result.Success(lunch);
    }

    public Guid TenantId { get; set; }
    public DateOnly Date { get; private set; }
    public Guid SchoolId { get; private set; }
    public Guid RestaurantId { get; private set; }
    public List<LunchItem> AvailablePacItems { get; init; } = [];
    public List<LunchItem> AvailableRestaurantItems { get; init; } = [];

    public void AddPacItem(LunchItem item) => AvailablePacItems.Add(item);
    public void AddRestaurantItem(LunchItem item) => AvailableRestaurantItems.Add(item);
}