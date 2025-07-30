// using System.Security.Claims;
// using Contracts.Schools.Responses;
// using Contracts.Users.DTOs;
// using Contracts.Users.Messages;
// using Contracts.Users.Responses;
// using Dapr.Client;
// using Users.Api.Domain;
// using Users.Api.EfCore;
//
// namespace Users.Api.Features;
//
// public static class Profile
// {
//     public static void MapProfile(this WebApplication app)
//     {
//         app.MapGet("/profile", async (
//                 ClaimsPrincipal claimsPrincipal,
//                 AppDbContext db,
//                 DaprClient dapr,
//                 CancellationToken ct) =>
//         {
//             // The 'oid' claim will be used for the user id, because it is the immutable ID of the user in Entra ID across all apps in the tenant.
//             if (!Guid.TryParse(claimsPrincipal.FindFirst("oid")?.Value, out var userId))
//             {
//                 return Results.Problem($"Claim 'oid' was not found in {nameof(claimsPrincipal)}.");
//             }
//             
//             var user = await db.Users.FindAsync([userId], ct);
//             if (user is null)
//             {
//                 var name = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
//                 var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value
//                             ?? claimsPrincipal.FindFirst("preferred_username")?.Value
//                             ?? "unknown";
//                 
//                 user = User.Create(userId, name, email);
//                 await db.Users.AddAsync(user, ct);
//                 await db.SaveChangesAsync(ct);
//                 
//                 await dapr.PublishEventAsync("pubsub", "user-events",
//                     new UserRegisteredMessage(user.Id, user.Name), ct);
//             }
//
//             List<UserSchoolDto> userSchools = [];
//             foreach (var sid in user.SchoolIds)
//             {
//                 var school = await dapr.InvokeMethodAsync<SchoolResponse>(HttpMethod.Get, "schools-api", $"/{sid}", ct);
//                 userSchools.Add(MapUserSchoolDto(school, user.Children));
//             }
//
//             var response = new UserProfileResponse(user.Id, user.Email, user.Name, [..userSchools]);
//             
//             return Results.Ok(response);
//         })
//         .WithName("Profile")
//         .WithSummary("Profile")
//         .WithTags("Users")
//         .Produces<UserProfileResponse>()
//         .Produces(StatusCodes.Status400BadRequest)
//         .Produces(StatusCodes.Status404NotFound);
//     }
//     
//     private static UserSchoolDto MapUserSchoolDto(SchoolResponse school, List<Child> children)
//     {
//         ChildDto[] childDtos =
//         [
//             .. children
//                 .Where(child => child.SchoolId == school.Id)
//                 .Select(child => new ChildDto(child.Id, child.Name, child.Grade))
//         ];
//
//         return new UserSchoolDto(school.Id, school.Name, childDtos);
//     }
// }