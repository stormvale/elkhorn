using Contracts.Schools.Messages;
using Contracts.Schools.Requests;
using Dapr.Client;
using Schools.Api.Domain;
using Schools.Api.EfCore;
using Schools.Api.Extensions;

namespace Schools.Api.Features;

public static class Register
{
    public static void MapRegister(this WebApplication app)
    {
        app.MapPost("/", async (RegisterSchoolRequest req, AppDbContext db, DaprClient dapr, CancellationToken ct) =>
        {
            var school = new School(Guid.CreateVersion7(),
                req.Name,
                req.Address.ToDomainAddress(),
                req.Contact.ToDomainContact());
            
            await db.Schools.AddAsync(school, ct);
            await db.SaveChangesAsync(ct);
            
            await dapr.PublishEventAsync("pubsub", "schools-events",
                new SchoolRegisteredMessage(school.Id, school.Name), ct);
            
            return TypedResults.CreatedAtRoute(
                school.Id,
                routeName: GetById.RouteName,
                routeValues: new { id = school.Id }
            );
        }).WithSummary("Register");
    }
}