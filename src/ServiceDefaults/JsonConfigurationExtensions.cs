using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ServiceDefaults;

public static class JsonConfigurationExtensions
{
    /// <summary>
    /// Adds JSON configuration services to the specified <see cref="IServiceCollection" />.
    /// Specifically, we are adding the <see cref="JsonStringEnumConverter"/> to both the AspNetCore
    /// HttpJsonOptions and the DaprClient JsonSerializationOptions.
    /// </summary>
    /// <param name="builder">The IHostApplicationBuilder instance.</param>
    /// <returns>The <see cref="IServiceCollection" /> with JSON configuration services added.</returns>
    public static void AddJsonConfiguration(this IHostApplicationBuilder builder)
    {
        var sharedOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        sharedOptions.Converters.Add(new JsonStringEnumConverter());
        
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            foreach (var converter in sharedOptions.Converters)
            {
                options.SerializerOptions.Converters.Add(converter);
            }
        });
        
        // TODO: should be made more explicit to the caller that this method adds the DaprClient.
        // probably move this somewhere else.
        builder.Services.AddDaprClient(config =>
        {
            config.UseJsonSerializationOptions(sharedOptions);
        });
    }
}