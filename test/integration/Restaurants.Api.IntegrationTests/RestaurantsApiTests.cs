using System.Net;
using System.Net.Http.Json;
using Contracts.Common.Responses;
using Contracts.Restaurants.Requests;
using Contracts.Restaurants.Responses;
using Microsoft.Azure.Cosmos;
using Restaurants.Api.Domain;
using Restaurants.Api.IntegrationTests.Factories;
using Restaurants.Api.IntegrationTests.Fixtures;
using Shouldly;

namespace Restaurants.Api.IntegrationTests;

public class RestaurantApiTests : IClassFixture<CosmosDbEmulatorFixture>
{
    private readonly HttpClient _restaurantsHttpClient;
    private readonly CosmosClient _cosmosClient;

    public RestaurantApiTests(CosmosDbEmulatorFixture cosmos)
    {
        var factory = new RestaurantApiFactory(cosmos.ConnectionString);
        _restaurantsHttpClient = factory.CreateClient();

        var options = new CosmosClientOptions
        {
            ConnectionMode = ConnectionMode.Gateway,
            HttpClientFactory = () => cosmos.Container.HttpClient
        };

        _cosmosClient = new CosmosClient(cosmos.ConnectionString, options);
    }

    [Fact]
    public async Task PostRestaurant_ThenGetById()
    {
        var request = new RegisterRestaurantRequest("Test Restaurant",
            new AddressResponse("street", "city", "postCode", "state"),
            new ContactResponse("name", "email", "phone", "Manager")
        );

        var postResult = await _restaurantsHttpClient.PostAsJsonAsync("/", request);
        postResult.StatusCode.ShouldBe(HttpStatusCode.Created);
        postResult.Headers.Location.ShouldNotBeNull();
        
        // check the response content
        var postResponse = await postResult.Content.ReadFromJsonAsync<RegisterRestaurantResponse>();
        postResponse.ShouldNotBeNull();
        postResponse.RestaurantId.ShouldNotBe(Guid.Empty);

        // // check the database directly (can't do this yet - db doesn't exist)
        // var container = _cosmosClient.GetContainer("TestDb", "restaurants");
        // var cosmosResult = await container.ReadItemAsync<Restaurant>(postResponse.RestaurantId.ToString(), new PartitionKey("id"));
        // cosmosResult.Resource.Name.ShouldBe(request.Name);

        var getResult = await _restaurantsHttpClient.GetAsync(postResponse.RestaurantId.ToString());
        postResult.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var restaurant = await getResult.Content.ReadFromJsonAsync<Restaurant>();
        restaurant.ShouldNotBeNull();
        restaurant.Id.ShouldNotBe(Guid.Empty);
        restaurant.Name.ShouldBe(request.Name);
    }
}