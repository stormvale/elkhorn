using Contracts.Lunches.Messages;
using Dapr;
using Notifications.Api.Services;

namespace Notifications.Api.Features;

public static class LunchScheduled
{
    public static void MapLunchScheduled(this WebApplication app)
    {
        app.MapPost("/lunch-scheduled", async (
                LunchScheduledMessage message,
                ILogger<Program> logger,
                EmailSender emailSender,
                CancellationToken ct) =>
            {
                logger.LogInformation("New Lunch ({LunchId}) scheduled for {Date}", message.LunchId, message.Date);

                await emailSender.SendEmailForLunchScheduled(message);

                return TypedResults.Ok();
            })
        .WithSummary("Lunch Scheduled")
        .WithTopic("pubsub", "lunch-events"); // subscribes to the topic
    }
}