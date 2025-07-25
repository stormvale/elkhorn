using Contracts.Users.Messages;
using Dapr.Client;
using Domain.Results;
using Users.Api.DomainErrors;
using Users.Api.EfCore;
using Users.Api.Extensions;

namespace Users.Api.Features;

public static class Delete
{
    public static void MapDelete(this WebApplication app)
    {
        app.MapDelete("/{userId:Guid}", async Task<IResult> (Guid userId, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var user = await db.Users.FindAsync([userId], ct);
            if (user is null)
            {
                return Result.Failure(UserErrors.NotFound(userId)).ToProblemDetails();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync(ct);

            await dapr.PublishEventAsync("pubsub", "user-events", new UserDeletedMessage(userId), ct);

            return TypedResults.NoContent();
        })
        .WithName("DeleteUser")
        .WithSummary("Delete User")
        .WithTags("Users")
        .RequireAuthorization()
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound);
    }
}