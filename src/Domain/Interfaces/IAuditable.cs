namespace Domain.Interfaces;

public interface IAuditable
{
    DateTimeOffset CreatedUtc { get; }

    DateTimeOffset? LastModifiedUtc { get; }
}
