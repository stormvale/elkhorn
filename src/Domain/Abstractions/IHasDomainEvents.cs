namespace Domain.Abstractions;

public interface IHasDomainEvents
{
    void AddDomainEvent(DomainEvent domainEvent);

    IReadOnlyList<DomainEvent> PopDomainEvents();
}

public record DomainEvent(DateTime RaisedAt);
