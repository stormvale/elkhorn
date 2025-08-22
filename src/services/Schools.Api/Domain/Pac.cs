using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Common;
using Domain.Results;
using Schools.Api.DomainErrors;

namespace Schools.Api.Domain;

public sealed class Pac : Entity
{
    [JsonConstructor] private Pac() : base(id: Guid.Empty) { /* ef constructor */ }
    
    public Pac(Guid id, Contact contact) : base(id)
    {
        Chairperson = contact;
    }

    public Contact Chairperson { get; set; }
    public List<PacFundraisingItem> LunchItems { get; set; } = [];
    
    public Result AddLunchItem(string name, decimal price)
    {
        if (LunchItems.Any(x => x.Name == name))
        {
            return Result.Failure(PacErrors.LunchItemAlreadyExists(name));
        }
        
        LunchItems.Add(new PacFundraisingItem(name, price));
        return Result.Success();
    }

    public Result RemoveLunchItem(string name)
    {
        LunchItems.RemoveAll(x => x.Name == name);
        return Result.Success();
    }
}