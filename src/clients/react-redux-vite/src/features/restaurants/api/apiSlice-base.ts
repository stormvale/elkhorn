import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'
import { RootState } from '../../../app/store';
import { getAccessTokenFromLocalStorage } from '../../../utils/tokenStorage';
import { msalInstance } from '../../../msalConfig';

// the endpoints for this api are generated from the OpenAPI spec
export const apiBase = createApi({
  baseQuery: fetchBaseQuery({
    baseUrl: import.meta.env.VITE_RESTAURANTS_API_URL,
    prepareHeaders: async (headers, { getState }) => {

      // first try to get the token from Redux state
      let token = (getState() as RootState).auth.accessToken;
      console.log('Token from Redux state:', token ? 'present' : 'not found');

      // if not in Redux state, then try localStorage
      if (!token) {
        token = getAccessTokenFromLocalStorage();
        console.log('Token from localStorage:', token ? 'present' : 'not found');
      }

      // still nothing. try get from MSAL silently
      if (!token) {
        try {
          const accounts = msalInstance.getAllAccounts();
          console.log('MSAL accounts found:', accounts.length);
          
          if (accounts.length > 0) {
            console.log('Attempting to get token from MSAL silently...');
            
            try {
              const tokenResponse = await msalInstance.acquireTokenSilent({
                scopes: ['api://f776afca-bc47-4fee-9c85-e86ee08578f5/RestaurantsApi.All'],
                account: accounts[0]
              });
              
              token = tokenResponse.accessToken;
              console.log('Token acquired from MSAL for restaurants API:', token ? 'success' : 'failed');
            } catch (msalError) {
              console.warn('Could not acquire token silently from MSAL:', msalError);
            }
          }
        } catch (error) {
          console.log('Could not access MSAL instance:', error);
        }
      }

      if (token) {
        console.log('✅ Setting Authorization header with token for restaurants API');
        headers.set('Authorization', `Bearer ${token}`);
      } else {
        console.warn('⚠️ No access token found for restaurants API request');
      }

      headers.set('Accept', 'application/json');
      return headers;
    }
  }),
  endpoints: () => ({})
})