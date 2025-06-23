namespace Contracts.Lunches.Messages;

// [Topic("lunch.scheduled")]
public record LunchScheduledMessage(Guid LunchId);
