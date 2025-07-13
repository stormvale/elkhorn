using Domain.Abstractions;

namespace Domain.Tests.SampleDomain;

internal sealed class Family(int id, Parent dad, Parent mom) : AggregateRoot<int>(id)
{
    public Parent Dad { get; set; } = dad;
    public Parent Mom { get; set; } = mom;
    public IEnumerable<Child> Children => Dad.Children.Concat(Mom.Children);
}
