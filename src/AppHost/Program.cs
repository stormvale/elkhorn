using CommunityToolkit.Aspire.Hosting.Dapr;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

#region Aspire resources

#pragma warning disable ASPIRECOSMOSDB001
// preview emulator

var cosmos = builder.AddAzureCosmosDB("cosmos")
    .RunAsPreviewEmulator(config =>
    {
        config.WithDataExplorer();
        config.WithLifetime(ContainerLifetime.Persistent);
    });

#pragma warning restore ASPIRECOSMOSDB001

var restaurantsDb = cosmos.AddCosmosDatabase("restaurantsDb");
var schoolsDb = cosmos.AddCosmosDatabase("schoolsDb");
var lunchesDb = cosmos.AddCosmosDatabase("lunchesDb");
var ordersDb = cosmos.AddCosmosDatabase("ordersDb");

restaurantsDb.AddContainer("restaurants", "/id");
schoolsDb.AddContainer("schools", "/id");
lunchesDb.AddContainer("lunches", "/id");
ordersDb.AddContainer("orders", "/id");

#endregion

#region Dapr components

var stateStore = builder.AddDaprStateStore("statestore");

var pubSub = builder.AddDaprPubSub("pubsub");

var email = builder.AddDaprComponent("email", "bindings.smtp", new DaprComponentOptions
{
    LocalPath = "components/"
});

var secretstore = builder.AddDaprComponent("secretstore", "secretstores.local.file", new DaprComponentOptions
{
    LocalPath = "components/"
});

#endregion

var restaurantsApi = builder.AddProject<Restaurants_Api>("restaurants-api")
    .WithDaprSidecar()
    .WithReference(stateStore)
    .WithReference(secretstore)
    .WithReference(pubSub)
    .WithReference(restaurantsDb);

var schoolsApi = builder.AddProject<Schools_Api>("schools-api")
    .WithDaprSidecar()
    .WithReference(stateStore)
    .WithReference(pubSub)
    .WithReference(schoolsDb);

var lunchesApi = builder.AddProject<Lunches_Api>("lunches-api")
    .WithDaprSidecar()
    .WithReference(stateStore)
    .WithReference(pubSub)
    .WithReference(lunchesDb);

// builder.AddProject<Projects.Cart_Api>("cart-api")
//     .WithDaprSidecar()
//     .WithReference(stateStore)
//     .WithReference(pubSub);

var ordersApi = builder.AddProject<Orders_Api>("orders-api")
    .WithDaprSidecar()
    .WithReference(stateStore)
    .WithReference(pubSub)
    .WithReference(ordersDb);

// builder.AddProject<Projects.Billing_Api>("billing-api")
//     .WithDaprSidecar()
//     .WithReference(stateStore)
//     .WithReference(pubSub);

builder.AddProject<Notifications_Api>("notifications-api")
    .WithDaprSidecar()
    .WithReference(stateStore)
    .WithReference(pubSub)
    .WithReference(email);

builder.AddProject<Gateway_Api>("gateway-api")
    .WithReference(restaurantsApi).WaitFor(restaurantsApi)
    .WithReference(schoolsApi).WaitFor(schoolsApi)
    .WithReference(lunchesApi).WaitFor(lunchesApi)
    .WithReference(ordersApi).WaitFor(ordersApi)
    .WithExternalHttpEndpoints()
    .WithUrlForEndpoint("https", url =>
    {
        url.DisplayText = "Scalar";
        url.Url = "/scalar";
    });

builder.Build().Run();
