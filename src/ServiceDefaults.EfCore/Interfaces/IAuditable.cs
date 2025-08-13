using ServiceDefaults.EfCore.Interceptors;

namespace ServiceDefaults.EfCore.Interfaces;

/// <summary>
/// Interface for entities that need auditing. Provides properties to track when an entity was created
/// and last modified. Properties are updated automatically by <see cref="UpdateAuditableInterceptor"/>,
/// which is already registered in 
/// </summary>
public interface IAuditable
{
    DateTimeOffset CreatedUtc { get; }

    DateTimeOffset? LastModifiedUtc { get; }
}
