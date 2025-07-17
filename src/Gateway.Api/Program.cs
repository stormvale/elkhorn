using System.Net.Http.Headers;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// add and configure reverse proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver()
    .AddTransforms(builderContext =>
    {
        builderContext.AddRequestTransform(transformContext =>
        {
            Console.WriteLine("Received transformContext");
            Console.WriteLine(transformContext.ToString());
            
            var authHeader = transformContext.HttpContext.Request.Headers.Authorization;
            if (!string.IsNullOrEmpty(authHeader))
            {
                transformContext.ProxyRequest.Headers.Authorization = AuthenticationHeaderValue.Parse(authHeader);
            }
            
            Console.WriteLine(transformContext.ToString());
            return ValueTask.CompletedTask;
        });
    });

// need to forward authentication headers to proxied api's
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://97919892-78d9-482f-a52e-55bfd7ae7c95.ciamlogin.com/97919892-78d9-482f-a52e-55bfd7ae7c95/v2.0";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false // forwarded tokens are not for us
        };
        options.IncludeErrorDetails = true;
    });

builder.Services.AddAuthorization();

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

// need to configure CORS to allow clients to call this API
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(corsPolicyBuilder => corsPolicyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    )
);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors();

// Handle preflight OPTIONS requests globally
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = StatusCodes.Status204NoContent;
        await context.Response.CompleteAsync();
        return;
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

app.MapDefaultEndpoints();
app.MapReverseProxy().RequireAuthorization();

app.Run();
