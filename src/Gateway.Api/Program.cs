using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseHttpsRedirection();

app.MapScalarApiReference(options =>
{
    options.AddDocument("restuarants-api", routePattern: "https://restaurants-api/api1/openapi/v1.json");
});

app.Run();
