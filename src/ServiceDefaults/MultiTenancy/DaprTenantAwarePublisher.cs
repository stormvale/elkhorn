using Dapr.Client;

namespace ServiceDefaults.MultiTenancy;

public class DaprTenantAwarePublisher(DaprClient daprClient, TenantContext tenantContext) : ITenantAwarePublisher
{
    public async Task PublishEventAsync<T>(string pubSubName, string topicName, T data, CancellationToken ct) where T : ITenantAware
    {
        data.TenantId = tenantContext.TenantId;
        await daprClient.PublishEventAsync(pubSubName, topicName, data, ct);
    }
}