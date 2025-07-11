using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Restaurants.Api.EfCore;

namespace Restaurants.Api.IntegrationTests.Factories;

public class RestaurantApiFactory(string cosmosConnectionString) : WebApplicationFactory<IRestaurantApiAssemblyMarker>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<AppDbContext>>();
            services.AddDbContext<AppDbContext>(options =>
                options.UseCosmos(cosmosConnectionString, databaseName: "TestDb"));
            
            services.AddDaprClient();
        });
    }
}