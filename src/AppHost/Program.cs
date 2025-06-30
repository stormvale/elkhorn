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

#endregion

#region Dapr components

var stateStore = builder.AddDaprStateStore("statestore");

var pubSub = builder.AddDaprPubSub("pubsub");

#endregion

var restaurantsApi = builder.AddProject<Projects.Restaurants_Api>("restaurants-api")
    .WithDaprSidecar()
    .WithReference(stateStore)
    .WithReference(pubSub)
    .WithReference(restaurantsDb);

var schoolsApi = builder.AddProject<Projects.Schools_Api>("schools-api")
    .WithDaprSidecar()
    .WithReference(stateStore)
    .WithReference(pubSub)
    .WithReference(schoolsDb);

var lunchesApi = builder.AddProject<Projects.Lunches_Api>("lunches-api")
    .WithDaprSidecar()
    .WithReference(stateStore)
    .WithReference(pubSub)
    .WithReference(lunchesDb);

// builder.AddProject<Projects.Cart_Api>("cart-api")
//     .WithDaprSidecar()
//     .WithReference(stateStore)
//     .WithReference(pubSub);
//
// builder.AddProject<Projects.Orders_Api>("orders-api")
//     .WithDaprSidecar()
//     .WithReference(stateStore)
//     .WithReference(pubSub);
//
// builder.AddProject<Projects.Billing_Api>("billing-api")
//     .WithDaprSidecar()
//     .WithReference(stateStore)
//     .WithReference(pubSub);

builder.AddProject<Projects.Notifications_Api>("notifications-api")
    .WithDaprSidecar()
    .WithReference(stateStore)
    .WithReference(pubSub);

builder.AddProject<Projects.Gateway_Api>("gateway-api")
    .WithReference(restaurantsApi).WaitFor(restaurantsApi)
    .WithReference(schoolsApi).WaitFor(schoolsApi)
    .WithExternalHttpEndpoints()
    .WithUrlForEndpoint("https", url =>
    {
        url.DisplayText = "Scalar";
        url.Url = "/scalar";
    });

builder.Build().Run();
