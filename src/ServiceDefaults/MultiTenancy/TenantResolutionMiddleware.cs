using Microsoft.AspNetCore.Http;

namespace ServiceDefaults.MultiTenancy;

public class TenantResolutionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, TenantContext tenantContext)
    {
        // Get the tenant ID from a custom header. This typically populated by YARP using a Transform to extract
        // the tenantId claim from the access token and forward it as this custom header.
        if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var headerValue))
        {
            var tenantId = headerValue.ToString();
            
            // Set the tenant ID on the scoped TenantContext.
            if (!string.IsNullOrEmpty(tenantId))
            {
                tenantContext.SetTenantId(tenantId);
            }
        }
            
        await next(context);
    }
}