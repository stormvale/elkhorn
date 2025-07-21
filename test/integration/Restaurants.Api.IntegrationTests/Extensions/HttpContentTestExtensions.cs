using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Restaurants.Api.IntegrationTests.Extensions;

public static class HttpContentTestExtensions
{
    private static readonly JsonSerializerOptions TestOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>
    /// Includes a JsonStringEnumConverter in the JsonSerializerOptions. This is needed when deserializing enum
    /// properties that have been serialized as strings (which the APIs tend to do.)
    /// </summary>
    public static Task<T> ReadFromJsonAsyncEnhanced<T>(this HttpContent content, CancellationToken ct = default)
    {
        return content.ReadFromJsonAsync<T>(TestOptions, ct)!;
    }
}