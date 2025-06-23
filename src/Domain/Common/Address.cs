using Domain.Abstractions;

namespace Domain.Common;

public sealed class Address : ValueObject
{
    private Address() { /* ef constructor */ }
    
    public Address(string street, string city, string postCode, string state)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(street);
        ArgumentException.ThrowIfNullOrWhiteSpace(city);
        ArgumentException.ThrowIfNullOrWhiteSpace(postCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(state);

        Street = street;
        City = city;
        PostCode = postCode;
        State = state;
    }

    // these property setters are important for ef to map the complex type
    
    public string Street { get; set; }
    public string City { get; set; }
    public string PostCode { get; set; }
    public string State { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PostCode;
        yield return State;
        yield return Street;
        yield return City;
    }

    public override string ToString() => $"{Street}, {City}, {PostCode}, {State}";
}
