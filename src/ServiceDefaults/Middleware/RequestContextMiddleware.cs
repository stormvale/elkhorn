using Microsoft.AspNetCore.Http;
using ServiceDefaults.Middleware.MultiTenancy;

namespace ServiceDefaults.Middleware;

public class RequestContextMiddleware(RequestDelegate next)
{
    /// <summary>
    /// Processes the HTTP request and populates the tenant and user context using the following
    /// custom HTTP headers:
    /// <list type="bullet">
    ///   <item>X-Tenant-Id</item>
    ///   <item>X-User-Id</item>
    ///   <item>X-User-Email</item>
    ///   <item>X-User-Roles</item>
    /// </list>
    /// </summary>
    /// <param name="httpContext">The current HTTP context.</param>
    /// <param name="requestContext">Accessor that will be configured with the current <see cref="RequestContext"/>.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext httpContext, IRequestContextAccessor requestContext)
    {
        var tenantContext = new TenantContext();
        var userContext = new UserContext();
        
        if (httpContext.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantId))
            tenantContext.SetTenantId(Guid.Parse(tenantId.ToString()));
        
        if (httpContext.Request.Headers.TryGetValue("X-User-Id", out var userId))
            userContext.UserId = Guid.Parse(userId.ToString());
        
        if (httpContext.Request.Headers.TryGetValue("X-User-Email", out var userEmail))
            userContext.Email = userEmail.ToString();
        
        if (httpContext.Request.Headers.TryGetValue("X-User-Roles", out var userRoles))
            userContext.Roles = userRoles.ToString().Split(',');

        requestContext.SetCurrent(new RequestContext
        {
            Tenant = tenantContext,
            User = userContext
        });
            
        await next(httpContext);
    }
}