using Dapr.Client;

namespace ServiceDefaults.MultiTenancy;

public class DaprTenantAwarePublisher(DaprClient daprClient, TenantContext tenantContext) : ITenantAwarePublisher
{
    public async Task PublishEventAsync<T>(string pubSubName, string topicName, T data, CancellationToken ct) where T : ITenantAware
    {
        data.TenantId = tenantContext.TenantId;
        
        Dictionary<string, string> metaData = new()
        {
            { "type", typeof(T).Name }
        };
        
        await daprClient.PublishEventAsync(pubSubName, topicName, data, metaData, ct);
    }
}

