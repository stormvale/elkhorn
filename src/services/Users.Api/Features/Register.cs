using Contracts.Users.Messages;
using Contracts.Users.Requests;
using Contracts.Users.Responses;
using Dapr.Client;
using Users.Api.Domain;
using Users.Api.EfCore;
using Users.Api.Extensions;

namespace Users.Api.Features;

public static class Register
{
    public static void MapRegister(this WebApplication app)
    {
        app.MapPost("/", async (RegisterUserRequest req, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var createUserResult = User.Create(req.Id, req.Name, req.Email);
            if (createUserResult.IsFailure)
            {
                return createUserResult.ToProblemDetails();
            }
            
            var user = createUserResult.Value!;
            await db.Users.AddAsync(user, ct);
            await db.SaveChangesAsync(ct);
            
            await dapr.PublishEventAsync("pubsub", "user-events",
                new UserRegisteredMessage(user.Id, user.Name), ct);
            
            return TypedResults.CreatedAtRoute(
                new RegisterUserResponse(user.Id),
                routeName: GetById.RouteName,
                routeValues: new { id = user.Id }
            );
        })
        .WithName("RegisterUser")
        .WithSummary("Register")
        .WithTags("Users")
        .Produces<RegisterUserResponse>(StatusCodes.Status201Created);
    }
}