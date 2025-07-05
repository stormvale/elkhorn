namespace Contracts.Schools.Messages;

public sealed record SchoolRegisteredMessage(Guid SchoolId, string Name);
