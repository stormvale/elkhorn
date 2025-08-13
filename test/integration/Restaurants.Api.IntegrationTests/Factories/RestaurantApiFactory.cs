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
using ServiceDefaults.EfCore.Interceptors;

namespace Restaurants.Api.IntegrationTests.Factories;

public class RestaurantApiFactory(string cosmosCosmosConnectionString) : WebApplicationFactory<IRestaurantApiAssemblyMarker>
{
    private readonly Lazy<HttpClient> _httpClient = new(() => new HttpClient(HttpClientHandler));
    
    private static readonly HttpClientHandler HttpClientHandler = new()
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
    
    public FakeDaprClient FakeDaprClient { get; private set; }

    public static Guid TestTenantId => Guid.Parse("4a79b424-eec8-4b1b-9309-d2987c8ac57f");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        
        //  tells the Cosmos DB SDK to skip certificate validation when connecting to the Cosmos DB Emulator
        AppContext.SetSwitch("Microsoft.Azure.Cosmos.AllowEmulatorUseWithoutLocalCertificate", true);
        
        builder.ConfigureTestServices(services =>
        {
            ConfigureEfCore(services);
            ConfigureFakeDapr(services);
            
            // for now, we are calling the api directly (not via gateway) so we don't need auth
            // ConfigureFakeAuthentication(services);
        });
    }

    /// <summary>
    /// Replaces the EF Core configuration with custom settings, including disabling SSL, Cosmos DB integration
    /// and tenant-specific interceptors.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
    private void ConfigureEfCore(IServiceCollection services)
    {
        // remove everything related to ef core
        var descriptorsToRemove = services.Where(d =>
            d.ServiceType.FullName?.Contains("EntityFrameworkCore") == true ||
            d.ServiceType.FullName?.Contains("AppDbContext") == true).ToList();
        
        foreach (var descriptor in descriptorsToRemove) services.Remove(descriptor);
        
        services.AddScoped<SetTenantIdInterceptor>();
        services.AddScoped<UpdateAuditableInterceptor>();

        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            options.UseCosmos(cosmosCosmosConnectionString, "TestDb", ConfigureCosmosDbContextNoSSL);

            var tenantInterceptor = serviceProvider.GetRequiredService<SetTenantIdInterceptor>();
            var auditInterceptor = serviceProvider.GetRequiredService<UpdateAuditableInterceptor>();

            options.AddInterceptors(tenantInterceptor, auditInterceptor);
        });
    }

    /// <summary>
    /// Configures authentication for testing purposes. Adds a fake authentication scheme
    /// using <see cref="FakeAuthHandler"/> containing fake claims.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
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
    /// Replaces the DaprClient with a fake for testing purposes
    /// </summary>
    private void ConfigureFakeDapr(IServiceCollection services)
    {
        services.RemoveAll<DaprClient>();
        FakeDaprClient = new FakeDaprClient();
        services.AddSingleton<DaprClient>(FakeDaprClient);
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