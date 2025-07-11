using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Restaurants.Api.IntegrationTests.Services;

public sealed class RabbitMqConsumer(string connectionString)
{
    private readonly ConnectionFactory _factory = new()
    {
        Uri = new Uri(connectionString)
    };

    public async Task BindToQueueAsync(string exchange, string queueName, string routingKey)
    {
        await using var connection = await _factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(exchange: "elkhorn-dead-letter-queue", type: ExchangeType.Fanout, durable: true);
        await channel.QueueDeclareAsync(queue: "elkhorn-dead-letter-queue", durable: true, exclusive: false, autoDelete: false);

        Dictionary<string, object?> queueArgs = new()
        {
            ["x-dead-letter-exchange"] = "elkhorn-dead-letter-queue"
        };

        await channel.ExchangeDeclareAsync(exchange: exchange, type: ExchangeType.Topic, durable: true);
        await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: queueArgs);
        await channel.QueueBindAsync(queue: queueName, exchange: exchange, routingKey: routingKey);
    }

    public async Task<ConsumeResult> TryConsumeAsync(string queueName, int timeoutSeconds = 5)
    {
        await using var connection = await _factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        TaskCompletionSource<ConsumeResult> messageReceived = new();
        AsyncEventingBasicConsumer consumer = new(channel);
        AsyncEventHandler<BasicDeliverEventArgs> callback = null;
        
        callback = (_, eventArgs) =>
        {
            consumer.ReceivedAsync -= callback;
                
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var routingKey = eventArgs.RoutingKey;

            Console.WriteLine($"Received '{routingKey}':'{message}'");
            messageReceived.SetResult(new ConsumeResult(true, routingKey, message));
            return messageReceived.Task;
        };
        
        consumer.ReceivedAsync += callback;

        await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);

        var succeeded = await Task.WhenAny(messageReceived.Task, Task.Delay(TimeSpan.FromSeconds(timeoutSeconds))) == messageReceived.Task;

        return succeeded ? await messageReceived.Task : new ConsumeResult(false);
    }

    public record ConsumeResult(bool Succeeded, string? RoutingKey = null, string? Message = null);
}
