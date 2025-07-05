namespace Contracts.Orders.Messages;

public record OrderCreatedMessage(
    Guid OrderId,
    Guid LunchId
);
