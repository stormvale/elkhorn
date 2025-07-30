using Contracts.Users.Messages;
using Contracts.Users.Requests;
using Contracts.Users.Responses;
using Dapr.Client;
using Domain.Results;
using Users.Api.Domain;
using Users.Api.DomainErrors;
using Users.Api.EfCore;
using Users.Api.Extensions;

namespace Users.Api.Features.Children;

public static class RegisterChild
{
    public static void MapRegisterChild(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (Guid userId, RegisterChildRequest req, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var user = await db.Users.FindAsync([userId], ct);
            if (user is null)
            {
                return Result.Failure(UserErrors.NotFound(userId)).ToProblemDetails();
            }

            var createChildResult = Child.Create(req.FirstName, req.LastName, userId, req.SchoolId, req.SchoolName, req.Grade);
            if (createChildResult.IsFailure)
            {
                return createChildResult.ToProblemDetails();
            }
            
            var registerChildResult = user.RegisterChild(createChildResult.Value!);
            if (registerChildResult.IsFailure)
            {
                return registerChildResult.ToProblemDetails();
            }

            var newChild = createChildResult.Value!;
            await db.SaveChangesAsync(ct);
            
            await dapr.PublishEventAsync("pubsub", "user-events",
                new ChildRegisteredMessage(user.Id, newChild.Id, newChild.SchoolId), ct);
            
            return TypedResults.Ok(newChild.ToChildResponse());
        })
        .WithName("RegisterChild")
        .WithSummary("Register Child")
        .WithTags("Users", "Children")
        .Produces<ChildResponse>();
    }
}