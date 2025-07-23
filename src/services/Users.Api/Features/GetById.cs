using Contracts.Users.Responses;
using Domain.Results;
using Users.Api.DomainErrors;
using Users.Api.EfCore;
using Users.Api.Extensions;

namespace Users.Api.Features;

public static class GetById
{
    public const string RouteName = "GetUserById";

    public static void MapGetById(this WebApplication app)
    {
        app.MapGet("/{id:Guid}", async (Guid id, AppDbContext db, CancellationToken ct) =>
        {
            var user = await db.Users.FindAsync([id], ct);
            
            return user is null 
                ? Result.Failure(UserErrors.NotFound(id)).ToProblemDetails()
                : TypedResults.Ok(user.ToUserResponse());
        })
        .WithName(RouteName)
        .WithSummary("Get by Id")
        .WithTags("Users")
        .Produces<UserResponse>()
        .Produces(StatusCodes.Status404NotFound);
    }
}