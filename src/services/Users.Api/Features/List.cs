using Contracts.Users.Responses;
using Microsoft.EntityFrameworkCore;
using Users.Api.EfCore;
using Users.Api.Extensions;

namespace Users.Api.Features;

public static class List
{
    public static void MapList(this WebApplication app)
    {
        app.MapGet("/", async Task<IReadOnlyList<UserResponse>> (AppDbContext db, CancellationToken ct) =>
        {
            var result = await db.Users.ToListAsync(ct);
            
            return result.Select(x => x.ToUserResponse())
                .ToList()
                .AsReadOnly();
        })
        .WithName("ListUsers")
        .WithSummary("List")
        .WithTags("Users")
        .Produces<List<UserResponse>>();
    }
}