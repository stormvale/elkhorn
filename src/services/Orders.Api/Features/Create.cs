using Contracts.Orders.Messages;
using Contracts.Orders.Requests;
using Dapr.Client;
using Orders.Api.Domain;
using Orders.Api.EfCore;
using Orders.Api.Extensions;

namespace Orders.Api.Features;

public static class Create
{
    public static void MapCreate(this WebApplication app)
    {
        app.MapPost("/", async (CreateOrderRequest req, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var order = new Order(
                Guid.CreateVersion7(),
                req.LunchId,
                req.Contact.ToContact()
            );
            
            await db.Orders.AddAsync(order, ct);
            await db.SaveChangesAsync(ct);
            
            await dapr.PublishEventAsync("pubsub", "order-events",
                new OrderCreatedMessage(order.Id, req.LunchId), ct);
            
            return TypedResults.CreatedAtRoute(
                order.Id,
                routeName: GetById.RouteName,
                routeValues: new { id = order.Id }
            );
        }).WithSummary("Create");
    }
}