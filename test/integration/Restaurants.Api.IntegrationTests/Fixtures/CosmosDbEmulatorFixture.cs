using DotNet.Testcontainers.Builders;
using Testcontainers.CosmosDb;

namespace Restaurants.Api.IntegrationTests.Fixtures;

public sealed class CosmosDbEmulatorFixture : IAsyncLifetime
{
    private readonly CosmosDbContainer _cosmosContainer = new CosmosDbBuilder()
        .WithImage("mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator:vnext-preview")
        .WithEnvironment("AZURE_COSMOS_EMULATOR_PARTITION_COUNT", "1")
        .WithEnvironment("AZURE_COSMOS_EMULATOR_ENABLE_DATA_PERSISTENCE", "false")
        .WithEnvironment("AZURE_COSMOS_EMULATOR_IP_ADDRESS_OVERRIDE", "127.0.0.1")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8081))
        .Build();

    // there is no SSL => no https
    public string ConnectionString => _cosmosContainer.GetConnectionString().Replace("https://", "http://");
    
    public async Task InitializeAsync() => await _cosmosContainer.StartAsync();

    public async Task DisposeAsync() => await _cosmosContainer.DisposeAsync();
}