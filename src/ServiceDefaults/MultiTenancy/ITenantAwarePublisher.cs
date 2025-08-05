namespace ServiceDefaults.MultiTenancy;

public interface ITenantAwarePublisher
{
    /// <summary>
    /// Publishes a message for a specific tenant using the configured Dapr client. The publishing service
    /// will automatically populate the TenantId property of the message using the current TenantContext.
    /// </summary>
    /// <param name="pubSubName">The Dapr pubsub component name.</param>
    /// <param name="topicName">The Dapr topic name.</param>
    /// <param name="data">The <see cref="ITenantAware"/> message to publish.</param>
    /// <param name="ct">The cancellation token which will be forwarded to the Dapr client.</param>
    Task PublishEventAsync<T>(string pubSubName, string topicName, T data, CancellationToken ct) where T : ITenantAware;
}