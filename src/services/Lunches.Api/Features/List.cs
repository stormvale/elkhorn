using Contracts.Lunches.Responses;
using Lunches.Api.EfCore;
using Lunches.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Lunches.Api.Features;

public static class List
{
    public static void MapList(this WebApplication app)
    {
        app.MapGet("/", async (AppDbContext db, CancellationToken ct) =>
        {
            var result = await db.Lunches.ToListAsync(ct);

            return result.Select(x => x.ToLunchResponse()).ToList();
        })
        .WithName("ListLunches")
        .WithSummary("List Lunches")
        .WithTags("Lunches")
        .Produces<List<LunchResponse>>();
    }
}