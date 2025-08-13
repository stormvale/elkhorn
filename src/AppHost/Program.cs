using CommunityToolkit.Aspire.Hosting.Dapr;
using Projects;
using Scalar.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

#region Aspire resources

#pragma warning disable ASPIRECOSMOSDB001
// preview emulator

var cosmos = builder.AddAzureCosmosDB("cosmos-db")
    .RunAsPreviewEmulator(config =>
    {
        config.WithDataExplorer();
        config.WithLifetime(ContainerLifetime.Persistent);
    });

#pragma warning restore ASPIRECOSMOSDB001

// normally we'd use a database per service, but to try to stay within the free tier limits on Azure,
// we are using a single database with a container per service.
var elkhornDb = cosmos.AddCosmosDatabase("elkhornDb");

elkhornDb.AddContainer("restaurants", ["/TenantId"]);
elkhornDb.AddContainer("schools", ["/TenantId"]);
elkhornDb.AddContainer("lunches", ["/TenantId"]);
elkhornDb.AddContainer("orders", ["/TenantId"]);
elkhornDb.AddContainer("users", ["/TenantId"]);

#endregion

#region Dapr components

var daprOptions = new DaprComponentOptions
{
    LocalPath = "components/"
};

var secretstore = builder.AddDaprComponent("secretstore", "secretstores.local.file", daprOptions);
var statestore = builder.AddDaprComponent("statestore", "state.redis", daprOptions);
var email = builder.AddDaprComponent("email", "bindings.smtp", daprOptions);
var pubSub = builder.AddDaprPubSub("pubsub", daprOptions);

#endregion

var restaurantsApi = builder.AddProject<Restaurants_Api>("restaurants-api")
    .WithDaprSidecar()
    .WithReference(statestore)
    .WithReference(secretstore)
    .WithReference(pubSub)
    .WithReference(cosmos);

var schoolsApi = builder.AddProject<Schools_Api>("schools-api")
    .WithDaprSidecar()
    .WithReference(pubSub)
    .WithReference(cosmos);

var lunchesApi = builder.AddProject<Lunches_Api>("lunches-api")
    .WithDaprSidecar(new DaprSidecarOptions
    {
        LogLevel = "debug",           // Options: debug, info, warn, error, fatal, panic
        EnableApiLogging = true       // Optional: enables API call logging
    })
    .WithReference(pubSub)
    .WithReference(cosmos);

var usersApi = builder.AddProject<Users_Api>("users-api")
    .WithDaprSidecar()
    .WithReference(pubSub)
    .WithReference(cosmos);

var ordersApi = builder.AddProject<Orders_Api>("orders-api")
    .WithDaprSidecar()
    .WithReference(pubSub)
    .WithReference(cosmos);

// builder.AddProject<Projects.Cart_Api>("cart-api")
//     .WithDaprSidecar()
//     .WithReference(stateStore)
//     .WithReference(pubSub);

// builder.AddProject<Projects.Billing_Api>("billing-api")
//     .WithDaprSidecar()
//     .WithReference(stateStore)
//     .WithReference(pubSub);

builder.AddProject<Notifications_Api>("notifications-api")
    .WithDaprSidecar()
    .WithReference(statestore)
    .WithReference(pubSub)
    .WithReference(email);

var gatewayApi = builder.AddProject<Gateway_Api>("gateway-api")
    .WithReference(restaurantsApi).WaitFor(restaurantsApi)
    .WithReference(schoolsApi).WaitFor(schoolsApi)
    .WithReference(lunchesApi).WaitFor(lunchesApi)
    .WithReference(ordersApi).WaitFor(ordersApi)
    .WithReference(usersApi).WaitFor(usersApi)
    .WithExternalHttpEndpoints();

#region Scalar OpenApi documentation

    var scalar = builder.AddScalarApiReference(options =>
    {
        options.WithTheme(ScalarTheme.Moon);
        options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });

    scalar
        .WithApiReference(restaurantsApi, options => options.WithOpenApiRoutePattern("/openapi/v1.json"))
        .WithApiReference(schoolsApi, options => options.WithOpenApiRoutePattern("/openapi/v1.json"))
        .WithApiReference(lunchesApi, options => options.WithOpenApiRoutePattern("/openapi/v1.json"))
        .WithApiReference(ordersApi, options => options.WithOpenApiRoutePattern("/openapi/v1.json"))
        .WithApiReference(usersApi, options => options.WithOpenApiRoutePattern("/openapi/v1.json"));

#endregion

// this was the first iteration of the web app (no longer used)
builder.AddNpmApp("test-ui", "../clients/test-ui")
    .WithReference(gatewayApi)
    .WithHttpEndpoint(name: "web", port: 54321, isProxied: false) // fixed port
    .WithEnvironment("VITE_PORT", "54321")
    .WithEnvironment("BROWSER", "none")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.AddNpmApp("react-redux-vite", "../clients/react-redux-vite")
    .WithReference(gatewayApi)
    .WithHttpEndpoint(name: "web", port: 57575, isProxied: false) // fixed port
    .WithEnvironment("VITE_PORT", "57575")
    .WithEnvironment("BROWSER", "none")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();