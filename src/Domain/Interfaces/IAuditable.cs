namespace Domain.Abstractions;

public interface IAuditable
{
    DateTimeOffset CreatedUtc { get; }

    DateTimeOffset? LastModifiedUtc { get; }
}
