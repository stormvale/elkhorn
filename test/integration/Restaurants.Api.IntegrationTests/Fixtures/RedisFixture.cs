using Testcontainers.Redis;

namespace Restaurants.Api.IntegrationTests.Fixtures;

public sealed class RedisFixture : IAsyncLifetime
{
    public RedisContainer Container { get; private set; }

    public string Host => Container.Hostname;
    public int Port => Container.GetMappedPublicPort(6379);

    public async ValueTask InitializeAsync()
    {
        Container = new RedisBuilder()
            .WithImage("redis:7")
            .WithCleanUp(true)
            .WithPortBinding(6379, true)
            .Build();

        await Container.StartAsync();
    }

    public async ValueTask DisposeAsync() => await Container.DisposeAsync();
}