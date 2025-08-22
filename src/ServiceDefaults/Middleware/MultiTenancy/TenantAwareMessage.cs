// namespace ServiceDefaults.Middleware.MultiTenancy;
//
// public abstract record TenantAwareMessage(Guid TenantId) : ITenantAware
// {
//     // (to enforce 'required' condition)
//     public required Guid TenantId { get; set; } = TenantId;
// }