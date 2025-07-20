using Dapr.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Restaurants.Api.EfCore;
using Restaurants.Api.IntegrationTests.Services;

namespace Restaurants.Api.IntegrationTests.Factories;

public class RestaurantApiFactory(string cosmosCosmosConnectionString) : WebApplicationFactory<IRestaurantApiAssemblyMarker>
{
    private readonly Lazy<HttpClient> _httpClient = new(() => new HttpClient(HttpClientHandler));
    
    private static readonly HttpClientHandler HttpClientHandler = new()
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        
        //  tells the Cosmos DB SDK to skip certificate validation when connecting to the Cosmos DB Emulator
        AppContext.SetSwitch("Microsoft.Azure.Cosmos.AllowEmulatorUseWithoutLocalCertificate", true);
        
        builder.ConfigureTestServices(services =>
        {
            ConfigureEfCore(services);

            ConfigureFakeAuthentication(services);
            
            // replace Dapr client with a TestDaprClient fake
            services.RemoveAll<DaprClient>();
            services.AddSingleton<DaprClient>(new FakeDaprClient());
        });
    }

    /// <summary>
    /// Replaces the EF Core configuration with one configured for the TestDb and also no SSL
    /// </summary>
    private void ConfigureEfCore(IServiceCollection services)
    {
        // remove everything related to ef core
        var descriptorsToRemove = services.Where(d =>
            d.ServiceType.FullName?.Contains("EntityFrameworkCore") == true ||
            d.ServiceType.FullName?.Contains("AppDbContext") == true).ToList();
        
        foreach (var descriptor in descriptorsToRemove) services.Remove(descriptor);
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseCosmos(cosmosCosmosConnectionString, "TestDb", ConfigureCosmosDbContextNoSSL));
    }

    /// <summary>
    /// Adds a fake authentication scheme using a fake authentication handler
    /// </summary>
    private static void ConfigureFakeAuthentication(IServiceCollection services)
    {
        // using a test auth scheme with known claims
        services.AddAuthentication("FakeAuth")
            .AddScheme<AuthenticationSchemeOptions, FakeAuthHandler>("FakeAuth", options => { });

        services.Configure<AuthenticationOptions>(options =>
        {
            options.DefaultAuthenticateScheme = "FakeAuth";
            options.DefaultChallengeScheme = "FakeAuth";
        });
    }

    /// <summary>
    /// Configure EF Core Cosmos options for SSL bypass
    /// </summary>
    private void ConfigureCosmosDbContextNoSSL(CosmosDbContextOptionsBuilder cosmosOptions)
    {
        cosmosOptions.ConnectionMode(ConnectionMode.Gateway);
        cosmosOptions.LimitToEndpoint();
        cosmosOptions.HttpClientFactory(() => _httpClient.Value);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_httpClient.IsValueCreated)
            {
                _httpClient.Value.Dispose();
            }
            HttpClientHandler.Dispose();
        }
        base.Dispose(disposing);
    }
}