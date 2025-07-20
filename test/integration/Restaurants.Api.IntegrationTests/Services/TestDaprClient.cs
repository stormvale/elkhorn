using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Dapr.Client;

// a bunch of the encrypt/decrypt stuff is obsolete
#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable CS0672 // Member overrides obsolete member

namespace Restaurants.Api.IntegrationTests.Services;

/// <summary>
/// Test implementation for dapr methods used in code.
///
/// Possible changes to consider:
///   - adding logging/counters/metrics for method calls.
///   - random exceptions/delays to simulate failure scenarios or latency.
/// </summary>
[SuppressMessage("Naming", "CA1725:Parameter names should match base declaration")]
public class TestDaprClient : DaprClient
{
    public IReadOnlyList<(string pubsub, string topic, object data)> PublishedEvents => _publishedEvents;
    public IReadOnlyDictionary<(string store, string key), object> StateStore => _stateStore;
    
    private readonly Dictionary<(string store, string key), object> _stateStore = [];
    private readonly List<(string pubsub, string topic, object data)> _publishedEvents = [];

    public override JsonSerializerOptions JsonSerializerOptions => new(JsonSerializerDefaults.Web);

    public override Task PublishEventAsync<TData>(string pubsubName, string topicName, TData data, CancellationToken ct = default)
    {
        _publishedEvents.Add((pubsubName, topicName, data));
        return Task.CompletedTask;
    }

    public override Task PublishEventAsync<TData>(string pubsubName, string topicName, TData data, Dictionary<string, string> metadata = null, CancellationToken ct = default)
    {
        PublishEventAsync(pubsubName, topicName, data, ct);
        return Task.CompletedTask;
    }

    public override Task PublishEventAsync(string pubsubName, string topicName, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task PublishEventAsync(string pubsubName, string topicName, Dictionary<string, string> metadata, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<BulkPublishResponse<TValue>> BulkPublishEventAsync<TValue>(string pubsubName, string topicName, IReadOnlyList<TValue> events, Dictionary<string, string> metadata = null!, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task PublishByteEventAsync(string pubsubName, string topicName, ReadOnlyMemory<byte> data, string dataContentType = "application/json", Dictionary<string, string> metadata = null!, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task InvokeBindingAsync<TRequest>(string bindingName, string operation, TRequest data, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<TResponse> InvokeBindingAsync<TRequest, TResponse>(string bindingName, string operation, TRequest data, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<BindingResponse> InvokeBindingAsync(BindingRequest request, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override HttpRequestMessage CreateInvokeMethodRequest(HttpMethod httpMethod, string appId, string methodName)
        => throw new NotImplementedException("Method not needed for tests");

    public override HttpRequestMessage CreateInvokeMethodRequest(HttpMethod httpMethod, string appId, string methodName, IReadOnlyCollection<KeyValuePair<string, string>> queryStringParameters)
        => throw new NotImplementedException("Method not needed for tests");

    public override HttpRequestMessage CreateInvokeMethodRequest<TRequest>(HttpMethod httpMethod, string appId, string methodName, IReadOnlyCollection<KeyValuePair<string, string>> queryStringParameters, TRequest data)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<bool> CheckHealthAsync(CancellationToken ct = default) => Task.FromResult(true);

    public override Task<bool> CheckOutboundHealthAsync(CancellationToken ct = default) => Task.FromResult(true);

    public override Task WaitForSidecarAsync(CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task ShutdownSidecarAsync(CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<DaprMetadata> GetMetadataAsync(CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task SetMetadataAsync(string attributeName, string attributeValue, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<HttpResponseMessage> InvokeMethodWithResponseAsync(HttpRequestMessage request, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override HttpClient CreateInvokableHttpClient(string? appId = null)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task InvokeMethodAsync(HttpRequestMessage request, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<TResponse> InvokeMethodAsync<TResponse>(HttpRequestMessage request, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task InvokeMethodGrpcAsync(string appId, string methodName, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task InvokeMethodGrpcAsync<TRequest>(string appId, string methodName, TRequest data, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<TResponse> InvokeMethodGrpcAsync<TResponse>(string appId, string methodName, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<TResponse> InvokeMethodGrpcAsync<TRequest, TResponse>(string appId, string methodName, TRequest data, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<TValue> GetStateAsync<TValue>(string storeName, string key, ConsistencyMode? consistencyMode = null, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
    {
        return (_stateStore.TryGetValue((storeName, key), out var value)
            ? Task.FromResult(value is TValue typedValue ? typedValue : default)
            : Task.FromResult(default(TValue)))!;
    }

    public override Task<IReadOnlyList<BulkStateItem>> GetBulkStateAsync(string storeName, IReadOnlyList<string> keys, int? parallelism, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<IReadOnlyList<BulkStateItem<TValue>>> GetBulkStateAsync<TValue>(string storeName, IReadOnlyList<string> keys, int? parallelism, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task SaveBulkStateAsync<TValue>(string storeName, IReadOnlyList<SaveStateItem<TValue>> items, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task DeleteBulkStateAsync(string storeName, IReadOnlyList<BulkDeleteStateItem> items, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<(TValue value, string etag)> GetStateAndETagAsync<TValue>(string storeName, string key, ConsistencyMode? consistencyMode = null, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task SaveStateAsync<TValue>(string storeName, string key, TValue value, StateOptions stateOptions = null!, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
    {
        _stateStore[(storeName, key)] = value;
        return Task.CompletedTask;
    }

    public override Task SaveByteStateAsync(string storeName, string key, ReadOnlyMemory<byte> binaryValue, StateOptions stateOptions = null!, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<bool> TrySaveByteStateAsync(string storeName, string key, ReadOnlyMemory<byte> binaryValue, string etag, StateOptions stateOptions = null!, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<ReadOnlyMemory<byte>> GetByteStateAsync(string storeName, string key, ConsistencyMode? consistencyMode = null, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<(ReadOnlyMemory<byte>, string etag)> GetByteStateAndETagAsync(string storeName, string key, ConsistencyMode? consistencyMode = null, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<bool> TrySaveStateAsync<TValue>(string storeName, string key, TValue value, string etag, StateOptions stateOptions = null!, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task ExecuteStateTransactionAsync(string storeName, IReadOnlyList<StateTransactionRequest> operations, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task DeleteStateAsync(string storeName, string key, StateOptions stateOptions = null!, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
    {
        _stateStore.Remove((storeName, key));
        return Task.CompletedTask;
    }

    public override Task<bool> TryDeleteStateAsync(string storeName, string key, string etag, StateOptions stateOptions = null!, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
    {
        var deleted = _stateStore.Remove((storeName, key));
        return Task.FromResult(deleted);
    }

    public override Task<StateQueryResponse<TValue>> QueryStateAsync<TValue>(string storeName, string jsonQuery, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<Dictionary<string, string>> GetSecretAsync(string storeName, string key, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<Dictionary<string, Dictionary<string, string>>> GetBulkSecretAsync(string storeName, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<GetConfigurationResponse> GetConfiguration(string storeName, IReadOnlyList<string> keys, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<SubscribeConfigurationResponse> SubscribeConfiguration(string storeName, IReadOnlyList<string> keys, IReadOnlyDictionary<string, string> metadata = null!, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<UnsubscribeConfigurationResponse> UnsubscribeConfiguration(string storeName, string id, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<ReadOnlyMemory<byte>> EncryptAsync(string vaultResourceName, ReadOnlyMemory<byte> plaintextBytes, string keyName, EncryptionOptions encryptionOptions, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<IAsyncEnumerable<ReadOnlyMemory<byte>>> EncryptAsync(string vaultResourceName, Stream plaintextStream, string keyName, EncryptionOptions encryptionOptions, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<ReadOnlyMemory<byte>> DecryptAsync(string vaultResourceName, ReadOnlyMemory<byte> ciphertextBytes, string keyName, DecryptionOptions options, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<ReadOnlyMemory<byte>> DecryptAsync(string vaultResourceName, ReadOnlyMemory<byte> ciphertextBytes, string keyName, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<IAsyncEnumerable<ReadOnlyMemory<byte>>> DecryptAsync(string vaultResourceName, Stream ciphertextStream, string keyName, DecryptionOptions options, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<IAsyncEnumerable<ReadOnlyMemory<byte>>> DecryptAsync(string vaultResourceName, Stream ciphertextStream, string keyName, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<TryLockResponse> Lock(string storeName, string resourceId, string lockOwner, int expiryInSeconds, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");

    public override Task<UnlockResponse> Unlock(string storeName, string resourceId, string lockOwner, CancellationToken ct = default)
        => throw new NotImplementedException("Method not needed for tests");
}