import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'
import { RootState } from '../../../app/store';
import { getAccessTokenFromLocalStorage } from '../../../utils/authStorage';
import { msalInstance } from '../../../msalConfig';

// the endpoints for this api are generated from the OpenAPI spec
export const apiBase = createApi({
  reducerPath: 'schoolsApi',
  baseQuery: fetchBaseQuery({
    baseUrl: import.meta.env.VITE_SCHOOLS_API_URL,
    prepareHeaders: async (headers, { getState }) => {
      let token: string | null = null;

      // first try to get token from MSAL silently
      const accounts = msalInstance.getAllAccounts();
      if (accounts.length > 0) {
        console.log('🔑 Attempting to get Schools API token from MSAL silently...');
        
        try {
          const tokenResponse = await msalInstance.acquireTokenSilent({
            scopes: ['api://2c25e4f8-a05f-4450-bbdd-ef927a3ed271/SchoolsApi.All'],
            account: accounts[0]
          });
          
          token = tokenResponse.accessToken;
        } catch (msalError) {
          console.warn('⚠️ Could not acquire Schools API token silently from MSAL:', msalError);
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
        console.warn('❗No token available to set Authorization header for Schools API request');
      }

      headers.set('Accept', 'application/json');
      return headers;
    }
  }),
  endpoints: () => ({})
})