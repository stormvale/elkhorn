using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Restaurants.Api.EfCore;
using Restaurants.Api.Features;
using Restaurants.Api.Features.Meals;
using Scalar.AspNetCore;
using ServiceDefaults;
using ServiceDefaults.EfCore;
using ServiceDefaults.Exceptions;
using ServiceDefaults.MultiTenancy;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddTenantServices();

builder.AddTenantAwareDbContext<AppDbContext>("cosmos-db", "elkhornDb");

// AddCosmosDbContext enables DbContext pooling. With pooling, the DbContext is configured from the root service provider where scoped services are not available.
// builder.AddCosmosDbContext<AppDbContext>("cosmos-db", "elkhornDb");
// builder.EnrichCosmosDbContext<AppDbContext>();

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

// Authorization policies go here...
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy =>
        policy.RequireAssertion(context =>
        {
            var httpContext = context.Resource as HttpContext;
            var role = httpContext?.Request.Headers["X-User-Role"].ToString();
            return role == "Admin";
        }));

builder.Services.AddProblemDetails(opt =>
{
    opt.CustomizeProblemDetails = ctx =>
        ctx.ProblemDetails.Extensions.TryAdd("requestId", ctx.HttpContext.TraceIdentifier);
});

builder.Services.ConfigureHttpJsonOptions(options => JsonExtensions.CreateJsonSerializerOptions());
builder.Services.AddDaprClient(config =>
{
    config.UseJsonSerializationOptions(JsonExtensions.CreateJsonSerializerOptions());
});

var app = builder.Build();

app.UseCloudEvents();
app.UseExceptionHandler();
app.UseTenantResolutionMiddleware();

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
