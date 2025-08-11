using System.Text.Json;
using System.Text.Json.Serialization;

namespace ServiceDefaults;

public static class JsonExtensions
{
    public static JsonSerializerOptions CreateJsonSerializerOptions()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());
        return options;
    }
}