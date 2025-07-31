using DotNet.Testcontainers.Builders;
using Restaurants.Api.IntegrationTests.Services;
using Testcontainers.RabbitMq;

namespace Restaurants.Api.IntegrationTests.Fixtures;

public sealed class RabbitMqFixture : IAsyncLifetime
{
    public RabbitMqContainer Container { get; private set; }
    public RabbitMqConsumer Consumer { get; private set; }

    public async ValueTask InitializeAsync()
    {
        Container = new RabbitMqBuilder()
            .WithImage("rabbitmq:latest")
            .WithUsername("guest")
            .WithPassword("guest")
            .WithPortBinding(5672, 5672)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
            .Build();
        
        await Container.StartAsync();
        
        // can only initialize the consumer once the container has started
        Consumer = new RabbitMqConsumer(Container.GetConnectionString());
        
    }

    public async ValueTask DisposeAsync() => await Container.DisposeAsync();
}