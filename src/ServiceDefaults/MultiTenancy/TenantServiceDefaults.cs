using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ServiceDefaults.MultiTenancy;

// Extension methods for tenant-related services
public static class TenantServiceDefaults
{
    /// <summary>
    /// Adds services required for multi-tenancy, including:
    /// <list type="bullet">
    ///   <item><term>scoped <see cref="TenantContext" />.</term></item>
    ///   <item><term>scoped <see cref="ITenantAwarePublisher" /> for DaprClient.</term></item>
    ///   <item><term>scoped <see cref="ITenantAwareServiceInvoker" /> for DaprClient.</term></item>
    /// </list>
    /// </summary>
    /// <param name="builder">The <see cref="IHostApplicationBuilder" /> to read config from and add services to.</param>
    public static void AddTenantServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<TenantContext>();
        builder.Services.AddScoped<ITenantAwarePublisher, DaprTenantAwarePublisher>();
        builder.Services.AddScoped<ITenantAwareServiceInvoker, DaprTenantAwareServiceInvoker>();
    }
    
    /// <summary>
    /// Adds middleware to retrieve the tenant ID from a custom header 'X-Tenant-Id' and use it to set the TenantId
    /// property on the TenantContext. The header is typically populated by the YARP Api Gateway using a Transform
    /// to extract the tenantId claim from the access token.
    /// </summary>
    public static IApplicationBuilder UseTenantResolutionMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<TenantResolutionMiddleware>();
    }
}