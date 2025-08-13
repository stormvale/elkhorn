using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ServiceDefaults;

public static class JsonConfigurationExtensions
{
    /// <summary>
    /// Adds JSON configuration services to the specified <see cref="IServiceCollection" />.
    /// Specifically, this adds the <see cref="JsonStringEnumConverter"/> to the AspNetCore
    /// HttpJsonOptions.
    /// </summary>
    /// <param name="builder">The IHostApplicationBuilder instance.</param>
    public static void AddJsonConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
    }
    
    /// <summary>
    /// Adds DaprClient services to the specified <see cref="IServiceCollection" /> with JSON
    /// configuration. Specifically, this adds the <see cref="JsonStringEnumConverter"/> to
    /// the DaprClient JsonSerializationOptions.
    /// </summary>
    /// <param name="builder">The IHostApplicationBuilder instance.</param>
    public static void AddDaprClientWithJsonConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDaprClient(config =>
        {
            var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            serializerOptions.Converters.Add(new JsonStringEnumConverter());
            
            config.UseJsonSerializationOptions(serializerOptions);
        });
    }
}