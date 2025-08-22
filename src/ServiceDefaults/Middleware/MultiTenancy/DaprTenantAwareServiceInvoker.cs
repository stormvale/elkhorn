using Dapr.Client;

namespace ServiceDefaults.Middleware.MultiTenancy;

public interface ITenantAwareServiceInvoker
{
    Task<TResponse> InvokeMethodAsync<TResponse>(HttpMethod httpMethod, string appId, string methodName, CancellationToken ct);
}

public class DaprTenantAwareServiceInvoker(DaprClient daprClient, IRequestContextAccessor requestContext) : ITenantAwareServiceInvoker
{
    public async Task<TResponse> InvokeMethodAsync<TResponse>(HttpMethod httpMethod, string appId, string methodName, CancellationToken ct = default)
    {
        if (httpMethod != HttpMethod.Get)
        {
            throw new NotSupportedException("Only GET is supported for Dapr service invocation for now.");
        }

        var request = daprClient.CreateInvokeMethodRequest(httpMethod, appId, methodName);
        request.Headers.Add("X-Tenant-Id", requestContext.Current.Tenant.TenantId.ToString());

        return await daprClient.InvokeMethodAsync<TResponse>(request, ct);
    }
}