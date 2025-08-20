using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceDefaults.Middleware.MultiTenancy;

namespace ServiceDefaults.Middleware;

public static class Extensions
{
    /// <summary>
    /// Registers services related to request context management.
    /// </summary>
    /// <param name="builder">The host application builder.</param>
    public static void AddRequestContextServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IRequestContextAccessor, RequestContextAccessor>();
    }

    /// <summary>
    /// Adds a <see cref="RequestContextMiddleware"/> to the application pipeline, which uses several
    /// custom HTTP headers to configure the scoped <see cref="IRequestContextAccessor"/>, which can
    /// be injected to access the current <see cref="TenantContext"/> or <see cref="UserContext"/>.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The application builder with the middleware added.</returns>
    public static IApplicationBuilder UseRequestContextMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestContextMiddleware>();
    }
}