import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'
import { RootState } from '../src/app/store';
import { getAccessTokenFromLocalStorage } from '../src/utils/tokenStorage';

// initialize an empty api service that we'll inject endpoints into later as needed
export const emptyApi = createApi({
  baseQuery: fetchBaseQuery({
    baseUrl: import.meta.env.VITE_RESTAURANTS_API_URL,
    prepareHeaders: (headers, { getState }) => {

      // first try to get the token from Redux state
      let token = (getState() as RootState).auth.accessToken;

      // if not in Redux state, then try localStorage
      if (!token) {
        token = getAccessTokenFromLocalStorage();
      }

      if (token) {
        headers.set('Authorization', `Bearer ${token}`);
      }

      headers.set('Accept', 'application/json');
      return headers;
    }
  }),
  endpoints: () => ({})
})