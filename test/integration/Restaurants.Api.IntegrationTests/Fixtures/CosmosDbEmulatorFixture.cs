using Testcontainers.CosmosDb;

namespace Restaurants.Api.IntegrationTests.Fixtures;

public sealed class CosmosDbEmulatorFixture : IAsyncLifetime
{
    public CosmosDbContainer Container { get; private set; }

    public string ConnectionString => Container.GetConnectionString();

    public async Task InitializeAsync()
    {
        Container = new CosmosDbBuilder()
            .WithImage("mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator:vnext-preview")
            .WithEnvironment("AZURE_COSMOS_EMULATOR_PARTITION_COUNT", "1")
            .WithEnvironment("AZURE_COSMOS_EMULATOR_ENABLE_DATA_PERSISTENCE", "false")
            .WithEnvironment("AZURE_COSMOS_EMULATOR_IP_ADDRESS_OVERRIDE", "127.0.0.1")
            .Build();

        await Container.StartAsync();
    }

    public async Task DisposeAsync() => await Container.DisposeAsync();
}