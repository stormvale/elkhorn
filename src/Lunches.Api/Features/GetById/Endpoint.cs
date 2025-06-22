using System.ComponentModel.DataAnnotations;
using Lunches.Api.Domain;
using Lunches.Api.Extensions;
using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Wolverine.Http;

namespace Lunches.Api.Features.GetById;

[Tags("Lunches")]
public static class Endpoint
{
    public const string RouteName = "GetLunchById";
    
    [WolverineGet("/api/lunches/{lunchId:Guid}", RouteName = RouteName)]
    [EndpointSummary("Get by Id")]
    public static async Task<Results<Ok<LunchResponse>, NotFound>> GetById(Guid lunchId, IQuerySession session)
    {
        var lunch = await session.Events.AggregateStreamAsync<Lunch>(lunchId);
        
        return lunch is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(lunch.ToLunchResponse());
    }
}

public sealed record LunchResponse(
    Guid LunchId,
    Guid SchoolId,
    Guid RestaurantId,
    DateOnly Date,
    List<LunchItemResponse> LunchItems);

public class LunchItemResponse(string name, decimal price)
{
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
    public string Name { get; set; } = name;

    [Required]
    [Range(0, 10, ErrorMessage = "Price must be between 0 and 10.")]
    public decimal Price { get; set; } = price;
}