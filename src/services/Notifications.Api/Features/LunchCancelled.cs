using Contracts.Lunches.Messages;
using Notifications.Api.Services;

namespace Notifications.Api.Features;

public static class LunchCancelled
{
    public static void MapLunchCancelled(this WebApplication app)
    {
        app.MapPost("/lunch-cancelled", async (
                LunchCancelledMessage message,
                ILogger<Program> logger,
                EmailSender emailSender,
                CancellationToken ct) =>
            {
                logger.LogInformation("Lunch {LunchId} was cancelled.", message.LunchId);

                await emailSender.SendEmailForLunchCancelled(message);

                return TypedResults.Ok();
            })
        .WithSummary("Lunch Cancelled")
        .WithTopic("pubsub", "lunch-events"); // subscribes to the topic
    }
}