using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Notifications.Api.Features;
using Notifications.Api.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi(o =>
{
    o.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "Notifications API",
            Version = "v1",
            Description = "Notifications API description.",
            Contact = new OpenApiContact { Name = "Kevin Reid", Url = new Uri("https://github.com/codeswithfists") },
            License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") },
            TermsOfService = new Uri("https://opensource.org/licenses/MIT")
        };
        
        return Task.CompletedTask;
    });

    // required if using Scalar.AspNetCore extensions package
    o.AddScalarTransformers();
});

builder.Services.AddDaprClient(config =>
{
    // let the dapr client know that enum values will be serialized as strings
    var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    config.UseJsonSerializationOptions(jsonSerializerOptions);
});

builder.Services.AddTransient<EmailSender>();

var app = builder.Build();

app.UseCloudEvents();
app.MapSubscribeHandler();
app.MapOpenApi();
app.MapDefaultEndpoints();

// endpoints
app.MapLunchScheduled();
app.MapLunchCancelled();

app.Run();
