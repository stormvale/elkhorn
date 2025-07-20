using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Contracts.Common;
using Contracts.Restaurants.Requests;
using Contracts.Restaurants.Responses;
using Restaurants.Api.IntegrationTests.Extensions;
using Restaurants.Api.IntegrationTests.Factories;
using Restaurants.Api.IntegrationTests.Fixtures;
using Shouldly;

namespace Restaurants.Api.IntegrationTests;

public class RestaurantApiTests : IClassFixture<CosmosDbEmulatorFixture>, IDisposable
{
    private readonly HttpClient _restaurantsHttpClient;
    //private readonly CosmosClient _cosmosClient;

    public RestaurantApiTests(CosmosDbEmulatorFixture cosmos)
    {
        var factory = new RestaurantApiFactory(cosmos.ConnectionString);
        
        _restaurantsHttpClient = factory.CreateClient();
        _restaurantsHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test", "test-token");

        // var options = new CosmosClientOptions
        // {
        //     ConnectionMode = ConnectionMode.Gateway,
        //     HttpClientFactory = () => cosmos.Container.HttpClient
        // };
        //
        // _cosmosClient = new CosmosClient(cosmos.ConnectionString, options);
    }

    [Fact]
    public async Task PostRestaurant_ThenGetById()
    {
        var request = new RegisterRestaurantRequest("Test Restaurant",
            new Address("123 test street", "city", "123456", "TS"),
            new Contact("name", "test@email.com", "1234567890", ContactType.Manager)
        );

        // POST request to the registration endpoint
        var postResult = await _restaurantsHttpClient.PostAsJsonAsync("/", request);
        postResult.StatusCode.ShouldBe(HttpStatusCode.Created);
        postResult.Headers.Location.ShouldNotBeNull();
        
        // check the response content
        var postResponse = await postResult.Content.ReadFromJsonAsync<RegisterRestaurantResponse>();
        postResponse.ShouldNotBeNull();
        postResponse.RestaurantId.ShouldNotBe(Guid.Empty);

        // GET request to the GetById endpoint
        var getResult = await _restaurantsHttpClient.GetAsync(postResponse.RestaurantId.ToString());
        getResult.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        // check the response content
        var restaurant = await getResult.Content.ReadFromJsonAsyncEnhanced<RestaurantResponse>();
        restaurant.ShouldNotBeNull();
        restaurant.Id.ShouldNotBe(Guid.Empty);
        restaurant.Name.ShouldBe(request.Name);
    }

    public void Dispose()
    {
        _restaurantsHttpClient.Dispose();
        // _cosmosClient.Dispose();
    }
}