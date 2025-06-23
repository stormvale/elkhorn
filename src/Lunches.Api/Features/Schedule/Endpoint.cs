using Contracts.Lunches.Requests;
using Contracts.Lunches.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Wolverine;
using Wolverine.Http;

namespace Lunches.Api.Features.Schedule;

[Tags("Lunches")]
public static class Endpoint
{
    [WolverinePost("/api/lunches")]
    [EndpointSummary("Schedule")]
    public static async Task<CreatedAtRoute<ScheduleLunchResponse>> Schedule(
        ScheduleLunchRequest request,
        IMessageBus bus,
        CancellationToken ct)
    {
        var command = Command.From(Guid.CreateVersion7(), request);

        await bus.InvokeAsync(command, ct);

        return TypedResults.CreatedAtRoute(
            new ScheduleLunchResponse(command.LunchId),
            routeName: GetById.Endpoint.RouteName,
            routeValues: new { lunchId = command.LunchId }
        );
    }
}