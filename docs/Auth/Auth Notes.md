
We are using Microsoft EntraId for our Identity Provider. Our APIs are using OAuth2 authentication implemented using JWT Bearer tokens. Once the user has obtained an access token, it needs to be included in the Authorization header on all requests to the API.

The API (in local dev at least) sits behind a YARP gateway. There is a transform that extracts the Authorization header from all incoming requests and attaches it to the proxied request to the API. 
#### Authority

The Authority is the URL of the trusted issuer (ie. Entra tenant) that the API uses to validate incoming access tokens.  We are using the 'issuer' value from the OpenID Connect metadata document for the App.  I think we may be able to also use https://login.microsoftonline.com/<TENANT_ID> => not tried yet.

```cs
options.Authority = "https://97919892-78d9-482f-a52e-55bfd7ae7c95.ciamlogin.com/97919892-78d9-482f-a52e-55bfd7ae7c95/v2.0";  
```

#### Audience
The Audience is the unique identifier of the API that the access token is intended for. In our case, this is the client Id of the Restaurants API app registered in Microsoft Entra.

```cs
options.Audience = "f776afca-bc47-4fee-9c85-e86ee08578f5";
```