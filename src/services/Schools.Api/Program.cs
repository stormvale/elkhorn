using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Schools.Api.EfCore;
using Schools.Api.Features;
using Schools.Api.Features.Pac;
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

// Using custom Header-based authorization policies
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("PacAdmin", policy =>
        policy.RequireAssertion(context =>
        {
            var httpContext = context.Resource as HttpContext;
            var roles = httpContext?.Request.Headers["X-User-Roles"].ToString().Split(',') ?? [];
            return roles.Contains("PacAdmin");
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

app.MapOpenApi();
app.MapDefaultEndpoints();
app.MapSubscribeHandler();

// endpoints
app.MapRegister();
app.MapGetById();
app.MapList();
app.MapDelete();

// pac endpoints (requires Admin or PacAdmin role)
var pac = app.MapGroup("{schoolId:Guid}/pac");
pac.MapCreateLunchItem();
pac.MapRemoveLunchItem();

await app.RunAsync();

namespace Schools.Api { public interface ISchoolsApiAssemblyMarker; }