namespace Domain.Exceptions;

public class AggregateNotFoundException : Exception
{
    public AggregateNotFoundException() { }

    public AggregateNotFoundException(string message) : base(message) { }

    public AggregateNotFoundException(string message, Exception innerException) : base(message, innerException) { }

    public AggregateNotFoundException(string typeName, string id) : base($"{typeName} with id '{id}' was not found") { }

    public static AggregateNotFoundException For<T>(Guid id) => new(typeof(T).Name, id.ToString());
}
