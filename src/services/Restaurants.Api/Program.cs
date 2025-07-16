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
    .AddJwtBearer(options =>
    {
        // The Authority is the URL of the trusted issuer (ie. Entra tenant) that the API uses to validate incoming access tokens.
        // We are using the 'issuer' value from the OpenID Connect metadata document for the App.
        // I think we may be able to also use https://login.microsoftonline.com/<TENANT_ID> => not tried yet.
        options.Authority = "https://97919892-78d9-482f-a52e-55bfd7ae7c95.ciamlogin.com/97919892-78d9-482f-a52e-55bfd7ae7c95/v2.0";
        
        // The Audience is the unique identifier of the API that the access token is intended for. In our case, this is the client Id
        // of the Restaurants API app registered in Microsoft Entra.
        options.Audience = "f776afca-bc47-4fee-9c85-e86ee08578f5";

        options.IncludeErrorDetails = true;
    });

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
