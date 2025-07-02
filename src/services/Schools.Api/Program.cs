using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Schools.Api.EfCore;
using Schools.Api.Features;
using Schools.Api.Features.Pac;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddCosmosDbContext<AppDbContext>("schoolsDb");

builder.Services.AddOpenApi(o =>
{
    o.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "Schools API",
            Version = "v1",
            Description = "Schools API description.",
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

var pac = app.MapGroup("{schoolId:Guid}/pac");
pac.MapCreateLunchItem();

await app.RunAsync();