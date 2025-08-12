namespace Domain.Abstractions;

public interface IAggregateRoot<out TId> : IEntity<TId>
{
    uint Version { get; }
}

/// <summary>
/// The default base AggregateRoot with a Guid identifier.
/// </summary>
public abstract class AggregateRoot(Guid id) : AggregateRoot<Guid>(id) { }

public abstract class AggregateRoot<TId>(TId id) : Entity<TId>(id), IAggregateRoot<TId>
    where TId : notnull
{
    /// <summary>
    /// Marten: automatically set if used as the target of a SingleStreamAggregation. Setter can't be private.
    /// EfCore: automatically set if mapped using 'IsRowVersion' in the configuration.
    /// </summary>
    public uint Version { get; set; }
}


