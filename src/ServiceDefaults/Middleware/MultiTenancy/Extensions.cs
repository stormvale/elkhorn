using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ServiceDefaults.Middleware.MultiTenancy;

public static class TenantServiceDefaults
{
    /// <summary>
    /// Adds DaprClient to the specified <see cref="IServiceCollection" /> with JSON
    /// configuration. Specifically, this adds the <see cref="JsonStringEnumConverter"/> to
    /// the DaprClient JsonSerializationOptions. Also adds the following:
    /// <list type="bullet">
    ///   <item>scoped <see cref="ITenantAwarePublisher" /> for DaprClient.</item>
    ///   <item>scoped <see cref="ITenantAwareServiceInvoker" /> for DaprClient.</item>
    ///   <item>scoped <see cref="ITenantAwareStateStore" /> for DaprClient.</item>
    /// </list>
    /// </summary>
    /// <param name="builder">The IHostApplicationBuilder instance.</param>
    public static void AddDaprClientAndTenantAwareServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDaprClient(config =>
        {
            var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            serializerOptions.Converters.Add(new JsonStringEnumConverter());
            
            config.UseJsonSerializationOptions(serializerOptions);
        });
        
        builder.Services.AddScoped<ITenantAwarePublisher, DaprTenantAwarePublisher>();
        builder.Services.AddScoped<ITenantAwareServiceInvoker, DaprTenantAwareServiceInvoker>();
        builder.Services.AddScoped<ITenantAwareStateStore, DaprTenantAwareStateStore>();
    }
}