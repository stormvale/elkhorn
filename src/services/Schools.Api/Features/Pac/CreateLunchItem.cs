using Contracts.Schools.Pac.Requests;
using Domain.Results;
using Schools.Api.DomainErrors;
using Schools.Api.EfCore;
using Schools.Api.Extensions;

namespace Schools.Api.Features.Pac;

public static class CreateLunchItem
{
    public static void MapCreateLunchItem(this RouteGroupBuilder group)
    {
        group.MapPost("/lunch-items", async (Guid schoolId, CreateLunchItemRequest req, AppDbContext db, CancellationToken ct) =>
        {
            var school = await db.Schools.FindAsync([schoolId], ct);
            if (school is null)
            {
                return Result.Failure(SchoolErrors.NotFound(schoolId)).ToProblemDetails();
            }

            var result = school.Pac.AddLunchItem(req.Name, req.Price);
            if (result.IsFailure)
            {
                return result.ToProblemDetails();
            }

            await db.SaveChangesAsync(ct);
            return TypedResults.Ok();
        })
        .WithName("CreatePacLunchItem")
        .WithSummary("Create PAC Lunch Item")
        .WithTags("Pac")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}