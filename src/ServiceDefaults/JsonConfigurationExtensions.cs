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
}