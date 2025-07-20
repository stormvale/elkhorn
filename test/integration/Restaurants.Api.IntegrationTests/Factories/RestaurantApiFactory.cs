using System.Diagnostics;
using System.Text.Json.Serialization;
using Dapr.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Restaurants.Api.EfCore;
using Restaurants.Api.IntegrationTests.Services;

namespace Restaurants.Api.IntegrationTests.Factories;

public class RestaurantApiFactory(string cosmosConnectionString) : WebApplicationFactory<IRestaurantApiAssemblyMarker>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        
        //  tells the Cosmos DB SDK to skip certificate validation when connecting to the Cosmos DB Emulator
        AppContext.SetSwitch("Microsoft.Azure.Cosmos.AllowEmulatorUseWithoutLocalCertificate", true);
        
        builder.ConfigureServices(services =>
        {
            // remove everything related to ef core
            var descriptorsToRemove = services.Where(d =>
                    d.ServiceType.FullName?.Contains("EntityFrameworkCore") == true ||
                    d.ServiceType.FullName?.Contains("AppDbContext") == true).ToList();
            foreach (var descriptor in descriptorsToRemove) services.Remove(descriptor);
            
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseCosmos(cosmosConnectionString, "TestDb", ConfigureEfCoreCosmosOptions);
            });
            
            InitializeCosmosDb().GetAwaiter().GetResult();
            
            // replace Dapr client with a TestDaprClient fake
            services.RemoveAll<DaprClient>();
            services.AddSingleton<DaprClient>(new TestDaprClient());
        });

        builder.ConfigureTestServices(services =>
        {
            // using a test auth scheme with known claims
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

            services.Configure<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            });
        });
    }
    
    /// <summary>
    /// Configure EF Core Cosmos options for SSL bypass
    /// </summary>
    private static void ConfigureEfCoreCosmosOptions(CosmosDbContextOptionsBuilder cosmosOptions)
    {
        cosmosOptions.ConnectionMode(ConnectionMode.Gateway);
        cosmosOptions.LimitToEndpoint();
        cosmosOptions.HttpClientFactory(() => new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        }));
    }
    
    /// <summary>
    /// Configure Azure Cosmos SDK client options for SSL bypass
    /// </summary>
    private static void ConfigureCosmosClientOptions(CosmosClientBuilder cosmosOptions)
    {
        cosmosOptions.WithConnectionModeGateway()
            .WithLimitToEndpoint(true)
            .WithHttpClientFactory(() => new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            }));
    }

    /// <summary>
    /// Create the Cosmos database and containers if they don't exist
    /// </summary>
    private async Task InitializeCosmosDb()
    {
        var cosmosOptionsBuilder = new CosmosClientBuilder(cosmosConnectionString);
        ConfigureCosmosClientOptions(cosmosOptionsBuilder);
        
        using var client = cosmosOptionsBuilder.Build();
        
        var database = await client.CreateDatabaseIfNotExistsAsync("TestDb");
        
        // create container (same container name as configured for DbContext)
        await database.Database.CreateContainerIfNotExistsAsync(
            id: "restaurants",
            partitionKeyPath: "/id",
            throughput: 400);
    }
}