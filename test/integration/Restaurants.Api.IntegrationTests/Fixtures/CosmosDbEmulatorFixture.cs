using DotNet.Testcontainers.Builders;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
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

    public string ConnectionString => _cosmosContainer.GetConnectionString()
        .Replace("https://", "http://"); // there is no SSL => no https

    public CosmosClient CosmosClient { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _cosmosContainer.StartAsync();

        CosmosClient = CreateCosmosClient();
        
        await CreateDatabaseIfNotExists(CosmosClient).ConfigureAwait(false);
    }

    public async Task DisposeAsync()
    {
        await _cosmosContainer.DisposeAsync();
        CosmosClient.Dispose();
    }

    /// <summary>
    /// Creates an Azure Cosmos SDK client with SSL bypassed
    /// </summary>
    private CosmosClient CreateCosmosClient()
    {
        var cosmosOptionsBuilder = new CosmosClientBuilder(ConnectionString)
            .WithConnectionModeGateway()
            .WithLimitToEndpoint(true)
            .WithHttpClientFactory(() =>
            {
                var httpClientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                
                return new HttpClient(httpClientHandler);
            });
        
        return cosmosOptionsBuilder.Build();
    }
    
    /// <summary>
    /// Create the Cosmos database and containers if they don't exist
    /// </summary>
    private static async Task CreateDatabaseIfNotExists(CosmosClient cosmosClient)
    {
        var database = await cosmosClient.CreateDatabaseIfNotExistsAsync("TestDb");
        
        // create container (same container name as configured for DbContext)
        await database.Database.CreateContainerIfNotExistsAsync(
            id: "restaurants",
            partitionKeyPath: "/id",
            throughput: 400);
    }
}