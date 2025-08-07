using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Identity.Web;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// add and configure YARP from the appsettings.json configuration
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver()
    .AddTransforms(builderContext =>
    {
        builderContext.AddRequestTransform(async transformContext =>
        {
            var user = transformContext.HttpContext.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                // when the access token is parsed into a ClaimsPrincipal, the 'tid' claim type is mapped to the full URI.
                var tenantId = user.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value;
                if (!string.IsNullOrEmpty(tenantId))
                {
                    // this is used by the TenantResolutionMiddleware to set the TenantId on the TenantContext
                    transformContext.ProxyRequest.Headers.Add("X-Tenant-ID", tenantId);
                }

                var roles = user.FindAll("roles").Select(r => r.Value);
                if (roles.Any())
                {
                    transformContext.ProxyRequest.Headers.Add("X-User-Roles", string.Join(",", roles));
                }
            }
        });
    });

// Default authorization policy which YARP will apply to incoming JWT tokens before executing any transforms
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
    //.AddJwtBearer(opt => builder.Configuration.Bind("JwtBearerOptions", opt));

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

// We need to configure CORS to allow clients to call this API.
// This "default" policy can be applied on routes in the YARP config.
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(corsPolicyBuilder => corsPolicyBuilder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials() // needed for JWT tokens
        .SetIsOriginAllowed(origin => true)
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
