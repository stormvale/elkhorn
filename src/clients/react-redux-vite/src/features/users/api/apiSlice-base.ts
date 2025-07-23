import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'
import { RootState } from '../../../app/store';
import { getAccessTokenFromLocalStorage } from '../../../utils/tokenStorage';
import { msalInstance } from '../../../msalConfig';

// the endpoints for this api are generated from the OpenAPI spec
export const apiBase = createApi({
  baseQuery: fetchBaseQuery({
    baseUrl: import.meta.env.VITE_USERS_API_URL,
    prepareHeaders: async (headers, { getState }) => {
      console.log('🔍 prepareHeaders called for users API');

      let token: string | null = null;

      // For Users API, we need a specific token with the correct audience
      // Try to get Users API token from MSAL silently first
      try {
        const accounts = msalInstance.getAllAccounts();
        console.log('👤 MSAL accounts found:', accounts.length);
        
        if (accounts.length > 0) {
          console.log('🔑 Attempting to get Users API token from MSAL silently...');
          
          try {
            const tokenResponse = await msalInstance.acquireTokenSilent({
              scopes: ['api://c8b4f2d6-2193-4338-b7bc-74f2ad75844e/UsersApi.All'],
              account: accounts[0]
            });
            
            token = tokenResponse.accessToken;
            console.log('✅ Users API token acquired from MSAL:', token ? 'success' : 'failed');
          } catch (msalError) {
            console.warn('⚠️ Could not acquire Users API token silently from MSAL:', msalError);
          }
        }
      } catch (error) {
        console.log('🚨 Could not access MSAL instance:', error);
      }

      // Fallback: try Redux state (but this might be MS Graph token)
      if (!token) {
        token = (getState() as RootState).auth.accessToken;
        console.log('📦 Fallback to token from Redux state:', token ? 'present' : 'not found');
      }

      // Last resort: try localStorage
      if (!token) {
        token = getAccessTokenFromLocalStorage();
        console.log('💾 Fallback to token from localStorage:', token ? 'present' : 'not found');
      }

      if (token) {
        console.log('✅ Setting Authorization header with token for Users API');
        console.log('🔑 Token details:', {
          length: token.length,
          starts: token.substring(0, 20) + '...',
          ends: '...' + token.substring(token.length - 20)
        });
        
        // Decode and log token payload for debugging
        try {
          const payload = JSON.parse(atob(token.split('.')[1]));
          console.log('📋 Users API Token payload:', {
            aud: payload.aud, // audience - should be your Users API
            iss: payload.iss, // issuer
            exp: new Date(payload.exp * 1000).toISOString(), // expiration
            scope: payload.scope || payload.scp, // scopes
            appid: payload.appid, // application ID
            azp: payload.azp // authorized party
          });
          
          // Check if this is the right audience
          const expectedAudience = 'api://c8b4f2d6-2193-4338-b7bc-74f2ad75844e';
          const isCorrectAudience = payload.aud === expectedAudience || 
                                   payload.aud === 'c8b4f2d6-2193-4338-b7bc-74f2ad75844e';
          console.log('🎯 Token audience check:', {
            expected: expectedAudience,
            actual: payload.aud,
            isCorrect: isCorrectAudience
          });
        } catch (e) {
          console.warn('Could not decode token payload:', e);
        }
        
        headers.set('Authorization', `Bearer ${token}`);
      } else {
        console.warn('⚠️ No access token found for Users API request');
      }

      headers.set('Accept', 'application/json');
      return headers;
    }
  }),
  endpoints: () => ({})
})