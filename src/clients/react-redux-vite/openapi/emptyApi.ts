import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'
import { RootState } from '../src/app/store';

// initialize an empty api service that we'll inject endpoints into later as needed
export const emptyApi = createApi({
  baseQuery: fetchBaseQuery({
    baseUrl: import.meta.env.VITE_RESTAURANTS_API_URL,
    prepareHeaders: (headers, { getState }) => {
      const token = (getState() as RootState).auth.accessToken;

      if (token) {
        headers.set('Authorization', 'Bearer ${token}');
      }

      return headers;
    }
  }),
  endpoints: () => ({})
})