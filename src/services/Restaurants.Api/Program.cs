using Microsoft.OpenApi.Models;
using Restaurants.Api.EfCore;
using Restaurants.Api.Features;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddCosmosDbContext<AppDbContext>("restaurantsDb");

builder.Services.AddOpenApi(o =>
{
    o.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "Restaurants API",
            Version = "v1",
            Description = "Restaurants API description.",
            Contact = new OpenApiContact { Name = "Kevin Reid", Url = new Uri("https://github.com/codeswithfists") },
            License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") },
            TermsOfService = new Uri("https://opensource.org/licenses/MIT")
        };
        
        return Task.CompletedTask;
    });

    // required if using Scalar.AspNetCore extensions package
    o.AddScalarTransformers();
});

builder.Services.AddDaprClient();

var app = builder.Build();

//app.UseHttpsRedirection();
app.UseCloudEvents();
app.MapSubscribeHandler();

app.MapOpenApi();
app.MapDefaultEndpoints();

// endpoints
app.MapRegister();
app.MapGetById();
app.MapList();

await app.RunAsync();
