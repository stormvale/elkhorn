using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Restaurants.Api.EfCore;
using Restaurants.Api.Features;
using Restaurants.Api.Features.Meals;
using Scalar.AspNetCore;
using ServiceDefaults.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddCosmosDbContext<AppDbContext>("cosmos-db", "elkhornDb");
builder.EnrichCosmosDbContext<AppDbContext>();

// if using multiple exception handlers, the order here matters
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt => builder.Configuration.Bind("JwtBearerOptions", opt));

// Authorization policies go here...
builder.Services.AddAuthorizationBuilder();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policyBuilder => policyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    )
);

builder.Services.AddDaprClient();
builder.Services.AddProblemDetails(opt =>
{
    opt.CustomizeProblemDetails = ctx =>
        ctx.ProblemDetails.Extensions.TryAdd("requestId", ctx.HttpContext.TraceIdentifier);
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    // enum values will be serialized as strings
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

//app.UseHttpsRedirection();
app.UseCors();
app.UseCloudEvents();
app.UseExceptionHandler();

app.MapOpenApi();
app.MapDefaultEndpoints();
app.MapSubscribeHandler();

// restaurant endpoints
app.MapRegister();
app.MapGetById();
app.MapList();
app.MapDelete();

// restaurant meal endpoints
var meals = app.MapGroup("{restaurantId:Guid}/meals");
meals.MapCreateMeal();
meals.MapDeleteMeal();

await app.RunAsync();

namespace Restaurants.Api { public interface IRestaurantApiAssemblyMarker; }
