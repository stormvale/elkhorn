using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using ServiceDefaults;
using ServiceDefaults.EfCore;
using ServiceDefaults.Exceptions;
using ServiceDefaults.MultiTenancy;
using Users.Api.EfCore;
using Users.Api.Features;
using Users.Api.Features.Children;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddTenantServices();
builder.AddJsonConfiguration();
builder.AddTenantAwareDbContext<AppDbContext>("cosmos-db", "elkhornDb");

// if using multiple exception handlers, the order here matters
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddOpenApi(o =>
{
    o.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "Users API",
            Version = "v1",
            Description = "Users API description.",
            Contact = new OpenApiContact { Name = "Kevin Reid", Url = new Uri("https://github.com/codeswithfists") },
            License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") },
            TermsOfService = new Uri("https://opensource.org/licenses/MIT")
        };
        
        return Task.CompletedTask;
    });

    o.AddScalarTransformers();
});

// Authorization policies go here...
builder.Services.AddAuthorizationBuilder();

builder.Services.AddProblemDetails(opt =>
{
    opt.CustomizeProblemDetails = ctx =>
        ctx.ProblemDetails.Extensions.TryAdd("requestId", ctx.HttpContext.TraceIdentifier);
});

var app = builder.Build();

app.UseCloudEvents();
app.UseExceptionHandler();
app.UseTenantResolutionMiddleware();

app.MapOpenApi();
app.MapDefaultEndpoints();
app.MapSubscribeHandler();

// endpoints
app.MapRegister();
app.MapGetById();
app.MapList();
app.MapDelete();

// children endpoints
var userChildren = app.MapGroup("{userId:Guid}/children");
userChildren.MapRegisterChild();
userChildren.MapUpdateChild();
userChildren.MapRemoveChild();

await app.RunAsync();

namespace Users.Api { public interface IUsersApiAssemblyMarker; }