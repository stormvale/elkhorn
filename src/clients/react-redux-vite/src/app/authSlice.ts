import { createSlice, PayloadAction } from '@reduxjs/toolkit';

/*
 * This slice manages authentication state, including the access token obtained via MSAL
 * and user information. It provides actions to set and clear credentials.
 */

interface AuthState {
  user: any | null;
  token: string | null;
  isAuthenticated: boolean;
}

const initialState: AuthState = {
  user: null,
  token: null,
  isAuthenticated: false
};

export const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setCredentials: (
      state,
      action: PayloadAction<{ accessToken: string; user: any }>
    ) => {
      // tenantProfiles is not serializable and should not be stored in Redux state
      const { tenantProfiles, ...userWithoutTenantProfiles } = action.payload.user;

      state.user = userWithoutTenantProfiles;
      state.token = action.payload.accessToken;
      state.isAuthenticated = true;
    },
    clearCredentials: (state) => {
      state.user = null;
      state.token = null;
      state.isAuthenticated = false;
    },
  },
});

export const { setCredentials, clearCredentials } = authSlice.actions;

export default authSlice.reducer;