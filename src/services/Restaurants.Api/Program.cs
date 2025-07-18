using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Restaurants.Api.EfCore;
using Restaurants.Api.Features;
using Restaurants.Api.Features.Meals;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddCosmosDbContext<AppDbContext>("cosmos-db", "elkhornDb");
builder.EnrichCosmosDbContext<AppDbContext>();

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
builder.Services.AddProblemDetails();

var app = builder.Build();

//app.UseHttpsRedirection();
//app.UseExceptionHandler();
app.UseCors();
app.UseCloudEvents();
app.MapSubscribeHandler();

app.MapOpenApi();
app.MapDefaultEndpoints();

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
