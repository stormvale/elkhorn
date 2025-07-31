using Dapr.Client;
using Domain.Results;
using Microsoft.AspNetCore.Mvc;
using Users.Api.DomainErrors;
using Users.Api.EfCore;
using Users.Api.Extensions;

namespace Users.Api.Features.Children;

public static class RemoveChild
{
    public static void MapRemoveChild(this RouteGroupBuilder group)
    {
        group.MapDelete("/{childId:Guid}", async (Guid userId, Guid childId, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var user = await db.Users.FindAsync([userId], ct);
            if (user is null)
            {
                return Result.Failure(UserErrors.NotFound(userId)).ToProblemDetails();
            }
        
            var removeChildResult = user.RemoveChild(childId);
            if (removeChildResult.IsFailure)
            {
                return removeChildResult.ToProblemDetails();
            }
            
            await db.SaveChangesAsync(ct);
            return TypedResults.Ok();
        })
        .WithName("RemoveChild")
        .WithSummary("Remove Child")
        .WithTags("Users", "Children")
        .Produces(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }
}