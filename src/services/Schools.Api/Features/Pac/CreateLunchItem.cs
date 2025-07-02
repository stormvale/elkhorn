using Contracts.Schools.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Schools.Api.EfCore;

namespace Schools.Api.Features.Pac;

public static class CreateLunchItem
{
    public static void MapCreateLunchItem(this RouteGroupBuilder group)
    {
        group.MapPost("/lunch-items", async Task<Results<Ok, ProblemHttpResult>> (Guid schoolId, CreateLunchItemRequest req, AppDbContext db, CancellationToken ct) =>
        {
            var school = await db.Schools.FindAsync([schoolId], ct);
            if (school is null)
            {
                return TypedResults.Problem(
                    detail: $"School with Id {schoolId} was not found",
                    statusCode: StatusCodes.Status404NotFound,
                    title: "School not found");
            }
            
            school.Pac.AddLunchItem(req.Name, req.Price);
            
            await db.SaveChangesAsync(ct);
            
            return TypedResults.Ok();
        });
    }
}