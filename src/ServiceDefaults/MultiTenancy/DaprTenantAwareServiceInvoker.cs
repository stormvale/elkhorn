using Dapr.Client;

namespace ServiceDefaults.MultiTenancy;

public interface ITenantAwareServiceInvoker
{
    Task<TResponse> InvokeMethodAsync<TResponse>(HttpMethod httpMethod, string appId, string methodName, CancellationToken ct);
}

public class DaprTenantAwareServiceInvoker(DaprClient daprClient, TenantContext tenantContext) : ITenantAwareServiceInvoker
{
    public async Task<TResponse> InvokeMethodAsync<TResponse>(HttpMethod httpMethod, string appId, string methodName, CancellationToken ct = default)
    {
        if (httpMethod != HttpMethod.Get)
        {
            throw new NotSupportedException("Only GET is supported for Dapr service invocation for now.");
        }

        var request = daprClient.CreateInvokeMethodRequest(httpMethod, appId, methodName);
        request.Headers.Add("X-Tenant-Id", tenantContext.TenantId.ToString());

        return await daprClient.InvokeMethodAsync<TResponse>(request, ct);
    }
}