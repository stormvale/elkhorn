namespace Contracts.Lunches.Messages;

public record LunchScheduledMessage(
    Guid LunchId,
    DateOnly Date,
    string SchoolName,
    string RestaurantName,
    string Route = "lunch-scheduled");
