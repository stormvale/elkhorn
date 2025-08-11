using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceDefaults.EfCore.Interceptors;

namespace ServiceDefaults.EfCore;

public static class Extensions
{
    /// <summary>
    /// Adds a tenant-aware DbContext to the application services.
    /// This will add a <see cref="SetTenantIdInterceptor"/> to the DbContextOptions, which can be used in
    /// OnModelCreating to configure a QueryFilter to always filter by the current TenantId.  
    /// </summary>
    /// <param name="builder">The IHostApplicationBuilder instance.</param>
    /// <param name="connectionStringName">The name of the connection string in the configuration file.</param>
    /// <param name="databaseName">The name of the database for the Cosmos DB context.</param>
    public static void AddTenantAwareDbContext<TContext>(
        this IHostApplicationBuilder builder,
        string connectionStringName,
        string databaseName) where TContext : DbContext
    {
        builder.Services.AddScoped<SetTenantIdInterceptor>();
        builder.Services.AddScoped<UpdateAuditableInterceptor>();

        builder.Services.AddDbContext<TContext>((serviceProvider, options) =>
        {
            var connectionString = builder.Configuration.GetConnectionString(connectionStringName)!;
            options.UseCosmos(connectionString, databaseName, cosmosOptions =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    // This configures the HttpClient used by the Cosmos DB client to trust the self-signed
                    // certificate from the local emulator. Required for local development with the Cosmos DB emulator.
                    cosmosOptions.HttpClientFactory(() => new HttpClient(new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    }));
                    cosmosOptions.ConnectionMode(Microsoft.Azure.Cosmos.ConnectionMode.Gateway);
                    cosmosOptions.LimitToEndpoint();
                }
            });

            var tenanatInterceptor = serviceProvider.GetRequiredService<SetTenantIdInterceptor>();
            var auditInterceptor = serviceProvider.GetRequiredService<UpdateAuditableInterceptor>();
            
            options.AddInterceptors(tenanatInterceptor, auditInterceptor);
        });
    }
}