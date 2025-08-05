using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Restaurants.Api.EfCore;
using Restaurants.Api.EfCore.Interceptors;
using Restaurants.Api.Features;
using Restaurants.Api.Features.Meals;
using Scalar.AspNetCore;
using ServiceDefaults.Exceptions;
using ServiceDefaults.MultiTenancy;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddTenantServices();

// register any EF Core interceptors (prefer singletons if possible) and inject them into the DbContext
builder.Services.AddScoped<SetTenantIdInterceptor>();
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionString = builder.Configuration.GetConnectionString("cosmos-db")!;
    options.UseCosmos(connectionString, "elkhornDb");

    var interceptor = serviceProvider.GetRequiredService<SetTenantIdInterceptor>();
    options.AddInterceptors(interceptor);
});

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

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policyBuilder => policyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    )
);

builder.Services.AddDaprClient(config =>
{
    // let the dapr client know that enum values will be serialized as strings
    var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    config.UseJsonSerializationOptions(jsonSerializerOptions);
});

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

app.UseCors();
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
