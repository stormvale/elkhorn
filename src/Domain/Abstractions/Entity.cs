using System.Text.Json.Serialization;

namespace Domain.Abstractions;

public interface IEntity<out TId>
{
    TId Id { get; }
}

/// <summary>
/// The default base Entity with a Guid entity id.
/// </summary>
public abstract class Entity(Guid id) : Entity<Guid>(id);

/// <summary>
/// The default base Entity
/// </summary>
public abstract class Entity<TId>(TId id) : IEntity<TId>, IEquatable<Entity<TId>>
    where TId : notnull
{
    // setter can't be private?? (check this)
    [JsonPropertyOrder(-1)] public virtual TId Id { get; init; } = id; 

    public virtual bool Equals(Entity<TId>? other) => Equals((object?)other);

    public override bool Equals(object? obj) => obj is Entity<TId> entity && Id.Equals(entity.Id);

    public static bool operator ==(Entity<TId> left, Entity<TId> right) => Equals(left, right);

    public static bool operator !=(Entity<TId> left, Entity<TId> right) => !(left == right);

    public override int GetHashCode() => Id.GetHashCode();
}


