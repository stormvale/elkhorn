using Domain.Abstractions;

namespace Schools.Api.Domain;

public sealed class PacFundraisingItem(string name, decimal price) : Entity(Guid.CreateVersion7())
{
    public string Name { get; init; } = name;

    public decimal Price { get; init; } = price;
}

