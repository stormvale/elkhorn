using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Schools.Api.EfCore;
using Schools.Api.Features;
using Schools.Api.Features.Pac;
using ServiceDefaults;
using ServiceDefaults.Exceptions;
using ServiceDefaults.EfCore;
using ServiceDefaults.Middleware;
using ServiceDefaults.Middleware.MultiTenancy;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddJsonConfiguration();
builder.AddRequestContextServices();
builder.AddDaprClientAndTenantAwareServices();
builder.AddTenantAwareDbContext<AppDbContext>("cosmos-db", "elkhornDb");

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

builder.Services.AddProblemDetails(opt =>
{
    opt.CustomizeProblemDetails = ctx =>
        ctx.ProblemDetails.Extensions.TryAdd("requestId", ctx.HttpContext.TraceIdentifier);
});

var app = builder.Build();

app.UseCloudEvents();
app.UseExceptionHandler();
app.UseRequestContextMiddleware();

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