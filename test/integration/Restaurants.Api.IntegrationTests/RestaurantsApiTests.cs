using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Contracts.Common;
using Contracts.Restaurants.Messages;
using Contracts.Restaurants.Requests;
using Contracts.Restaurants.Responses;
using Microsoft.Azure.Cosmos;
using Restaurants.Api.IntegrationTests.Extensions;
using Restaurants.Api.IntegrationTests.Factories;
using Restaurants.Api.IntegrationTests.Fixtures;
using Restaurants.Api.IntegrationTests.Services;
using Shouldly;

namespace Restaurants.Api.IntegrationTests;

public class RestaurantApiTests : IClassFixture<CosmosDbEmulatorFixture>, IDisposable
{
    private readonly HttpClient _restaurantsHttpClient;
    private readonly CosmosClient _cosmosClient;
    private readonly FakeDaprClient _fakeDaprClient;
    
    public RestaurantApiTests(CosmosDbEmulatorFixture cosmos)
    {
        var factory = new RestaurantApiFactory(cosmos.ConnectionString);
        
        _restaurantsHttpClient = factory.CreateClient();
        _restaurantsHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test", "test-token");
        _cosmosClient = cosmos.CosmosClient;
        _fakeDaprClient = factory.FakeDaprClient;
    }

    [Fact]
    public async Task WhenRegisterRequestIsValid_ResponseIsCorrect_RestaurantIsPersistedToDb()
    {
        var request = new RegisterRestaurantRequest("Test Restaurant",
            new Address("123 test street", "city", "123456", "TS"),
            new Contact("name", "test@email.com", "1234567890", ContactType.Manager)
        );

        // POST request to the registration endpoint
        var postResult = await _restaurantsHttpClient.PostAsJsonAsync("/", request, cancellationToken: TestContext.Current.CancellationToken);
        postResult.StatusCode.ShouldBe(HttpStatusCode.Created);
        postResult.Headers.Location.ShouldNotBeNull();
        
        // check the response content
        var postResponse = await postResult.Content.ReadFromJsonAsync<RegisterRestaurantResponse>(TestContext.Current.CancellationToken);
        postResponse.ShouldNotBeNull();
        postResponse.RestaurantId.ShouldNotBe(Guid.Empty);
        
        // inspect messages sent via Dapr
        _fakeDaprClient.PublishedEvents.Count.ShouldBe(1);
        var message = _fakeDaprClient.PublishedEvents[0].data as RestaurantRegisteredMessage;
        message.ShouldNotBeNull();
        message.RestaurantId.ShouldBe(postResponse.RestaurantId);
        
        // fetch resource directly from Cosmos DB
        var container = _cosmosClient.GetContainer("TestDb", "restaurants");
        var cosmosResponse = await container.ReadItemAsync<dynamic>(
            postResponse.RestaurantId.ToString(),
            new PartitionKey(postResponse.RestaurantId.ToString()),
            cancellationToken: TestContext.Current.CancellationToken);

        // verify the persisted data
        ((string)cosmosResponse.Resource["Name"]).ShouldBe(request.Name);
        ((string)cosmosResponse.Resource["Address"]["Street"]).ShouldBe(request.Address.Street);
        
        // GET request to the GetById endpoint
        var getResult = await _restaurantsHttpClient.GetAsync(postResponse.RestaurantId.ToString(), TestContext.Current.CancellationToken);
        getResult.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        // check the response content
        var restaurant = await getResult.Content.ReadFromJsonAsyncEnhanced<RestaurantResponse>(ct: TestContext.Current.CancellationToken);
        restaurant.ShouldNotBeNull();
        restaurant.Id.ShouldNotBe(Guid.Empty);
        restaurant.Name.ShouldBe(request.Name);
    }

    public void Dispose()
    {
        _restaurantsHttpClient.Dispose();
    }
}