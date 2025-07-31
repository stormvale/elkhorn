using Contracts.Users.Requests;
using Dapr.Client;
using Domain.Results;
using Microsoft.AspNetCore.Mvc;
using Users.Api.DomainErrors;
using Users.Api.EfCore;
using Users.Api.Extensions;

namespace Users.Api.Features.Children;

public static class UpdateChild
{
    public static void MapUpdateChild(this RouteGroupBuilder group)
    {
        group.MapPut("/{childId:Guid}", async (Guid userId, Guid childId, ChildUpsertRequest req, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var user = await db.Users.FindAsync([userId], ct);
            if (user is null)
            {
                return Result.Failure(UserErrors.NotFound(userId)).ToProblemDetails();
            }
        
            var child = user.Children.FirstOrDefault(x => x.Id == childId);
            if (child is null)
            {
                return Result.Failure(UserErrors.ChildNotFound(userId, childId)).ToProblemDetails();
            }
            
            child.UpdatePersonalInfo(req.FirstName, req.LastName);
            child.UpdateSchoolInfo(req.SchoolId, req.SchoolName, req.Grade);
            await db.SaveChangesAsync(ct);
            
            return TypedResults.Ok();
        })
        .WithName("UpdateChild")
        .WithSummary("Update Child")
        .WithTags("Users", "Children")
        .Produces(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }
}