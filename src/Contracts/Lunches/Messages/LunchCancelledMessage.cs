namespace Contracts.Lunches.Messages;

public record LunchCancelledMessage(Guid LunchId, string Route = "lunch-cancelled");
