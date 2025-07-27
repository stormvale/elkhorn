
#### Authenticate at the Gateway

- Configure YARP to use JWT Bearer authentication:
- Add auth middleware before proxying:

```csharp
app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy().RequireAuthorization();
```
  
#### Internal Api's will not require authentication.

If we want to **disable authentication on internal microservices** and rely solely on the **API Gateway for auth**, then we need to set the ingress for all API Container Apps to **“Limited to Container Apps Environment”**. This means that the **Api Gateway must live inside the same Container Apps environment** to reach those services directly.
#### Is this secure?

When we set ingress to **Limited to Container Apps Environment**:

- The Container App is **not exposed to the public internet**
- Only other apps **within the same Container Apps environment** can reach it
- This lets us **skip JWT validation** on internal services, since they’re shielded by the environment boundary
#### Forward Identity Claims

- If the internal services still need user context (e.g. user ID, roles), YARP can forward claims as headers:

```json
"Transforms": [
  { "RequestHeader": "X-User-Id", "Set": "{ClaimsPrincipal.Name}" },
  { "RequestHeader": "X-User-Role", "Set": "{ClaimsPrincipal.Role}" }
]
```

- The microservices can then read these headers without validating tokens.
#### What About Azure API Management?

Cool service and we'll still use it for documentation purposes, but Azure API Management (APIM) lives **outside** the Container Apps Environment, so it **can’t reach apps with internal-only ingress** unless we:

1. **Deploy APIM inside a VNet**
2. **Deploy the Container Apps environment as internal**, with ingress set to **Limited to VNet**

This setup requires:

- VNet integration for both APIM and Container Apps
- Private DNS zone for internal routing
- Possibly a premium APIM tier (for VNet support) <-- dealbreaker for now

#### Alternative

We will  **deploy a Gateway API into the Container Apps environment** and use it as your edge entry point, thereby avoiding the need for APIM.
- Deploy a **Gateway API container app** with **external ingress**
- Deploy all other microservices with **internal ingress**
- Authenticate at the Gateway, then use **Dapr service invocation** to call internal services