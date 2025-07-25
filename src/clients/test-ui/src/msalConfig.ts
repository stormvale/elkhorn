import { BrowserCacheLocation, Configuration, LogLevel, PublicClientApplication } from "@azure/msal-browser";

// MSAL uses response_type=code and does not support other responseTypes, as the recommended
// and more secure flow for single-page applications (SPAs) is the Authorization Code Flow
// with PKCE (Proof Key for Code Exchange), which uses response_type=code.

export const msalConfig: Configuration = {
    auth: {
      clientId: "c062b71e-d6d3-41df-8e9c-f70929e77d1e",

      // the authority is specifically the Elkhorn tenant
      authority: "https://97919892-78d9-482f-a52e-55bfd7ae7c95.ciamlogin.com/97919892-78d9-482f-a52e-55bfd7ae7c95/v2.0",

      // This must match exactly one of the URIs registered in Microsoft Entra => App Registration => Authentication.
      redirectUri: 'http://localhost:54321/signin-oidc',

      // the page to navigate after logout.
      postLogoutRedirectUri: '/',

      // If true, will navigate back to the original request location before processing the auth code response.
      navigateToLoginRequestUrl: false, 
    },

    cache: {
      cacheLocation: BrowserCacheLocation.LocalStorage,
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

/**
 * Scopes you add here will be requested during sign-in. By default, MSAL.js will add
 * OIDC scopes (openid, profile, email) to any login request. For more information about OIDC scopes, visit: 
 * https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-permissions-and-consent#openid-connect-scopes
 */
export const loginRequest = {
    scopes: [
        "api://f776afca-bc47-4fee-9c85-e86ee08578f5/RestaurantsApi.All"
    ]
};

/**
 * Add here the scopes to request when obtaining an access token for MS Graph API. For more information, see:
 * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/resources-and-scopes.md
 */
export const graphConfig = {
    graphMeEndpoint: "https://graph.microsoft.com/v1.0/me",
};