import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'
import { RootState } from '../../../app/store';
import { getAccessTokenFromLocalStorage } from '../../../utils/tokenStorage';
import { msalInstance } from '../../../msalConfig';

// the endpoints for this api are generated from the OpenAPI spec
export const apiBase = createApi({
  baseQuery: fetchBaseQuery({
    baseUrl: import.meta.env.VITE_USERS_API_URL,
    prepareHeaders: async (headers, { getState }) => {
      let token: string | null = null;

      // first try to get token from MSAL silently
      const accounts = msalInstance.getAllAccounts();
      if (accounts.length > 0) {
        console.log('🔑 Attempting to get Users API token from MSAL silently...');
        
        try {
          const tokenResponse = await msalInstance.acquireTokenSilent({
            scopes: ['api://c8b4f2d6-2193-4338-b7bc-74f2ad75844e/UsersApi.All'],
            account: accounts[0]
          });
          
          token = tokenResponse.accessToken;
        } catch (msalError) {
          console.warn('⚠️ Could not acquire Users API token silently from MSAL:', msalError);
        }
      }

      // Fallback: try Redux state (but this might be MS Graph token)
      if (!token) {
        token = (getState() as RootState).auth.accessToken;
        console.log('Fallback to token from Redux state:', token ? 'present' : 'not found');
      }

      // Last resort: try localStorage
      if (!token) {
        token = getAccessTokenFromLocalStorage();
        console.log('Fallback to token from localStorage:', token ? 'present' : 'not found');
      }

      if (token) {
        headers.set('Authorization', `Bearer ${token}`);
      } else {
        console.warn('No token available to set Authorization header for Restaurants API request');
      }

      headers.set('Accept', 'application/json');
      return headers;
    }
  }),
  endpoints: () => ({})
})