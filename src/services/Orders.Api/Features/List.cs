using Contracts.Orders.Responses;
using Microsoft.EntityFrameworkCore;
using Orders.Api.EfCore;
using Orders.Api.Extensions;

namespace Orders.Api.Features;

public static class List
{
    public static void MapList(this WebApplication app)
    {
        app.MapGet("/", async Task<IReadOnlyList<OrderResponse>> (AppDbContext db, CancellationToken ct) =>
        {
            var result = await db.Orders.ToListAsync(ct);
            
            return result.Select(x => x.ToOrderResponse())
                .ToList()
                .AsReadOnly();
            
        }).WithSummary("List");
    }
}