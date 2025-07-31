// using Contracts.Users.Messages;
// using Dapr.Client;
// using Domain.Results;
// using Users.Api.DomainErrors;
// using Users.Api.EfCore;
// using Users.Api.Extensions;
//
// namespace Users.Api.Features;
//
// public static class LinkSchool
// {
//     public static void MapLinkSchool(this WebApplication app)
//     {
//         app.MapPost("/{userId:Guid}/schools/{schoolId:Guid}", async (Guid userId, Guid schoolId, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
//         {
//             var user = await db.Users.FindAsync([userId], ct);
//             if (user is null)
//             {
//                 return Result.Failure(UserErrors.NotFound(userId)).ToProblemDetails();
//             }
//
//             var result = user.LinkSchool(schoolId);
//             if (result.IsFailure)
//             {
//                 return result.ToProblemDetails();
//             }
//
//             await db.SaveChangesAsync(ct);
//             await dapr.PublishEventAsync("pubsub", "user-events",
//                 new UserLinkedToSchoolMessage(user.Id, schoolId), ct);
//
//             return TypedResults.Ok();
//         })
//         .WithName("LinkUserToSchool")
//         .WithSummary("LinkUserToSchool")
//         .WithTags("Users", "Schools")
//         .Produces(StatusCodes.Status200OK)
//         .Produces(StatusCodes.Status401Unauthorized)
//         .Produces(StatusCodes.Status404NotFound);
//     }
// }