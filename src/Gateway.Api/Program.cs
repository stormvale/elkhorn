using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// add and configure reverse proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

builder.Services.AddRateLimiter(options =>
{
    // not sure why, but the default seems to be 503 Service Unavailable. 429 Too Many Requests is more correct.
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter("defaultPolicy", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromSeconds(10);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRateLimiter();

app.MapDefaultEndpoints();
app.MapReverseProxy();

app.MapScalarApiReference(options =>
{
    options.AddDocument("restaurants-api", routePattern: "https://localhost:7025/restaurants/openapi/v1.json");
    options.AddDocument("schools-api", routePattern: "https://localhost:7025/schools/openapi/v1.json");
    options.AddDocument("lunches-api", routePattern: "https://localhost:7025/lunches/openapi/v1.json");
    options.AddDocument("orders-api", routePattern: "https://localhost:7025/orders/openapi/v1.json");
    
    options.Servers =
    [
        new ScalarServer("https://localhost:7025/restaurants", "Restaurants API"),
        new ScalarServer("https://localhost:7025/schools", "Schools API"),
        new ScalarServer("https://localhost:7025/lunches", "Lunches API"),
        new ScalarServer("https://localhost:7025/orders", "Orders API")
    ];
    
    options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.Run();
