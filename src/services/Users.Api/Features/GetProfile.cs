using System.Security.Claims;
using Contracts.Users.DTOs;
using Contracts.Users.Responses;
using Domain.Results;
using Microsoft.EntityFrameworkCore;
using Users.Api.DomainErrors;
using Users.Api.EfCore;
using Users.Api.Extensions;

namespace Users.Api.Features;

public static class GetProfile
{
    public static void MapGetProfile(this WebApplication app)
    {
        app.MapGet("/profile", async (ClaimsPrincipal user, AppDbContext db, CancellationToken ct) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                         user.FindFirst("sub")?.Value ??
                         user.FindFirst("oid")?.Value; // Entra ID object identifier
            
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Problem("User ID not found in token", statusCode: 401);
            }
            
            var dbUser = await db.Users.FindAsync([userId], ct);

            var response = new UserProfileResponse(
                "abc123",
                "bob@email.com",
                "Bob",
                "Admin",
                "Bob Admin",
                new[]
                {
                    new UserSchoolDto("291", "Rock City Elementary", [
                        new ChildDto("child1", "Billy", "Kindergarten")
                    ]),
                    new UserSchoolDto("299", "Uplands Park Elementary", [
                        new ChildDto("child2", "Sammy", "3rd Grade")
                    ])
                },
                DateTime.UtcNow);
            
            return Results.Ok(response);
        })
        .WithName("Profile")
        .WithSummary("Profile")
        .WithTags("Users")
        .RequireAuthorization()
        .Produces<UserProfileResponse>()
        .Produces(StatusCodes.Status404NotFound);
    }
}