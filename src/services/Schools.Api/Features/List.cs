using Contracts.Schools.Responses;
using Microsoft.EntityFrameworkCore;
using Schools.Api.EfCore;
using Schools.Api.Extensions;

namespace Schools.Api.Features;

public static class List
{
    public static void MapList(this WebApplication app)
    {
        app.MapGet("/", async Task<IReadOnlyList<SchoolResponse>> (AppDbContext db, CancellationToken ct) =>
        {
            var result = await db.Schools.ToListAsync(ct);
            
            return result.Select(x => x.ToSchoolResponse())
                .ToList()
                .AsReadOnly();
        })
        .WithName("ListSchools")
        .WithSummary("List")
        .WithTags("Schools")
        .Produces<List<SchoolResponse>>();
    }
}