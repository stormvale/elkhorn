import { Configuration, LogLevel, PublicClientApplication } from "@azure/msal-browser";

// MSAL uses response_type=code and does not support other responseTypes, as the recommended
// and more secure flow for single-page applications (SPAs) is the Authorization Code Flow
// with PKCE (Proof Key for Code Exchange), which uses response_type=code.

export const msalConfig: Configuration = {
    auth: {
      clientId: "c062b71e-d6d3-41df-8e9c-f70929e77d1e",
      authority: "https://97919892-78d9-482f-a52e-55bfd7ae7c95.ciamlogin.com/97919892-78d9-482f-a52e-55bfd7ae7c95/v2.0",
      redirectUri: "http://localhost:57575/signin-oidc"
    },
    cache: {
      cacheLocation: "localStorage",
      storeAuthStateInCookie: false
    },
    system: {
    loggerOptions: {
      loggerCallback: (level, message, containsPii) => {
        if (containsPii) {
          return; // Do not log PII
        }
        switch (level) {
          case LogLevel.Error:
            console.error(message);
            return;
          case LogLevel.Info:
            console.info(message);
            return;
          case LogLevel.Verbose:
            console.debug(message);
            return;
          case LogLevel.Warning:
            console.warn(message);
            return;
        }
      }
    }
  }
};

export const msalInstance = new PublicClientApplication(msalConfig);

export const loginRequest = {
    scopes: [
        "User.Read", // Example scope for Microsoft Graph API
        "api://f776afca-bc47-4fee-9c85-e86ee08578f5/RestaurantsApi.All"
    ]
};

export const protectedResources = {
  graphApi: {
    endpoint: "https://graph.microsoft.com/v1.0/me",
    scopes: ["User.Read"],
  }
  // configurations for other protected APIs...
};