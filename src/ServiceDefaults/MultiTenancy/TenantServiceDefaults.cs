using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ServiceDefaults.MultiTenancy;

// Extension methods for tenant-related services
public static class TenantServiceDefaults
{
    public static IHostApplicationBuilder AddTenantServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<TenantContext>();
        builder.Services.AddScoped<ITenantAwarePublisher, DaprTenantAwarePublisher>();

        return builder;
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