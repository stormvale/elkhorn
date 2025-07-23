using Contracts.Schools.Responses;
using Domain.Results;
using Schools.Api.DomainErrors;
using Schools.Api.EfCore;
using Schools.Api.Extensions;

namespace Schools.Api.Features;

public static class GetById
{
    public const string RouteName = "GetById";

    public static void MapGetById(this WebApplication app)
    {
        app.MapGet("/{id:Guid}", async (Guid id, AppDbContext db, CancellationToken ct) =>
        {
            var school = await db.Schools.FindAsync([id], ct);
            
            return school is null 
                ? Result.Failure(SchoolErrors.NotFound(id)).ToProblemDetails()
                : TypedResults.Ok(school.ToSchoolResponse());
        })
        .WithName(RouteName)
        .WithSummary("Get by Id")
        .WithTags("Schools")
        .Produces<SchoolResponse>()
        .Produces(StatusCodes.Status404NotFound);
    }
}