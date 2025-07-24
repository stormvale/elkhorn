using System.Security.Claims;
using Contracts.Users.DTOs;
using Contracts.Users.Messages;
using Contracts.Users.Responses;
using Dapr.Client;
using Users.Api.Domain;
using Users.Api.EfCore;

namespace Users.Api.Features;

public static class Profile
{
    public static void MapProfile(this WebApplication app)
    {
        app.MapGet("/profile", async (ClaimsPrincipal claimsPrincipal, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            // ClaimTypes.NameIdentifier represents a standard claim type used in claims-based identity systems,
            // primarily to represent a unique and persistent identifier for a user within a specific context.
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Problem($"Claim '{ClaimTypes.NameIdentifier}' was not found in provided token.", statusCode: 401);
            }
            
            var user = await db.Users.FindAsync([userId], ct);
            if (user is null)
            {
                var name = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
                var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value
                            ?? claimsPrincipal.FindFirst("preferred_username")?.Value
                            ?? "unknown";
                
                user = User.Create(userId, name, email);
                await db.Users.AddAsync(user, ct);
                await db.SaveChangesAsync(ct);
                
                await dapr.PublishEventAsync("pubsub", "user-events",
                    new UserRegisteredMessage(user.Id, user.Name), ct);
            }

            var response = user.ToProfileResponse();
            
            return Results.Ok(response);
        })
        .WithName("Profile")
        .WithSummary("Profile")
        .WithTags("Users")
        .RequireAuthorization()
        .Produces<UserProfileResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);
    }
    
    public static UserProfileResponse ToProfileResponse(this User user)
    {
        var userSchoolsWithKids = user.SchoolIds.Select(schoolId => new UserSchoolDto(
            schoolId,
            $"School {schoolId}", 
            user.Children
                .Where(child => child.SchoolId == Guid.Parse(schoolId))
                .Select(child => new ChildDto(child.Id, child.Name, child.Grade))
                .ToArray()
        )).ToArray();
            
        return new UserProfileResponse(
            user.Id,
            user.Email,
            user.Name,
            userSchoolsWithKids);
    }
}