using Microsoft.OpenApi.Models;
using Orders.Api.EfCore;
using Orders.Api.Features;
using Scalar.AspNetCore;
using ServiceDefaults;
using ServiceDefaults.EfCore;
using ServiceDefaults.Exceptions;
using ServiceDefaults.Middleware;
using ServiceDefaults.Middleware.MultiTenancy;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddJsonConfiguration();
builder.AddRequestContextServices();
builder.AddDaprClientAndTenantAwareServices();
builder.AddTenantAwareDbContext<AppDbContext>("cosmos-db", "elkhornDb");

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddOpenApi(o =>
{
    o.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "Orders API",
            Version = "v1",
            Description = "Orders API description.",
            Contact = new OpenApiContact { Name = "Kevin Reid", Url = new Uri("https://github.com/codeswithfists") },
            License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") },
            TermsOfService = new Uri("https://opensource.org/licenses/MIT")
        };
        
        return Task.CompletedTask;
    });

    // required if using Scalar.AspNetCore extensions package
    o.AddScalarTransformers();
});

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
app.MapCreate();
app.MapGetById();
app.MapList();

await app.RunAsync();

namespace Orders.Api { public interface IOrdersApiAssemblyMarker; }
