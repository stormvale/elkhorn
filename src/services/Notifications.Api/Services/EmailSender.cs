using Dapr.Client;
using Contracts.Lunches.Messages;

namespace Notifications.Api.Services;

public class EmailSender(DaprClient daprClient, ILogger<EmailSender> logger)
{
    public async Task SendEmailForLunchScheduled(LunchScheduledMessage message)
    {
        logger.LogInformation("Received a LunchScheduledMessage for: {MessageLunchId}", message.LunchId);
        
        var metadata = new Dictionary<string, string>
        {
            ["emailTo"] = "kevinreid2023@outlook.com",
            ["subject"] = $"New hot-lunch scheduled for {message.Date}"
        };
        var body = $"""
                    <h2>New hot-lunch scheduled!</h2>

                    <p>Date: {message.Date}</p>
                    <p>School: {message.SchoolName}</p>
                    <p>Restaurant: {message.RestaurantName}</p>
                    
                    <p>Click <a href='https://localhost:7025/lunches/{message.LunchId}'>here</a> to order.</p>
                    <p>Ordering will close 3 days before at midnight on {message.Date.AddDays(-3)}</p>
                    """;
        
        await daprClient.InvokeBindingAsync("email", "create", body, metadata);
    }
    
    public async Task SendEmailForLunchCancelled(LunchCancelledMessage message)
    {
        logger.LogInformation("Received a LunchCancelledMessagefor: {MessageLunchId}", message.LunchId);
        
        var metadata = new Dictionary<string, string>
        {
            ["emailTo"] = "kevinreid2023@outlook.com",
            ["subject"] = $"Hot-lunch cancelled"
        };
        var body = $"""
                    <h2>Hot-lunch cancelled :-(</h2>

                    <p>Unfortunately, the upcoming hot-lunch with id {message.LunchId} has been cancelled.</p>

                    <p>No more orders will be accepted. Any existing orders will be refunded.</p>
                    """;
        
        await daprClient.InvokeBindingAsync("email", "create", body, metadata);
    }
}
