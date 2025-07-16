import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { User } from '../types/user';
import { tokenStorage } from '../utils/tokenStorage';

// Initialize state from localStorage
const initializeAuthState = (): AuthState => {
  const { accessToken, user } = tokenStorage.getAuthState();
  const isTokenValid = accessToken ? tokenStorage.isTokenValid() : false;

  if (isTokenValid && user) {
    return {
      isAuthenticated: true,
      user,
      accessToken,
      error: null,
    };
  } else {
    tokenStorage.clearAuthData();
    return {
      isAuthenticated: false,
      user: null,
      accessToken: null,
      error: null,
    };
  }
};

const initialState: AuthState = initializeAuthState();

/*
 * This slice manages authentication state, including the access token obtained via MSAL
 * and user information. It provides actions to set and clear credentials.
 */


interface AuthState {
  isAuthenticated: boolean;
  user: User | null;
  accessToken: string | null;
  error: string | null;
}

interface SetCredentialsPayload {
  accessToken: string;
  user: User;
}

export const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setCredentials: (state, action: PayloadAction<SetCredentialsPayload>) => {
      const { accessToken, user } = action.payload;

      state.isAuthenticated = true;
      state.user = user;
      state.accessToken = accessToken;
      state.error = null;

      tokenStorage.setAccessToken(accessToken);
      tokenStorage.setUser(user);
    },

    clearCredentials: (state) => {
      state.isAuthenticated = false;
      state.user = null;
      state.accessToken = null;
      state.error = null;
      tokenStorage.clearAuthData();
    },

    setAuthError: (state, action: PayloadAction<string>) => {
      state.error = action.payload;
    },

    restoreAuthStateFromLocalStorage: (state) => {
      const { accessToken, user } = tokenStorage.getAuthState();
      const isTokenValid = accessToken ? tokenStorage.isTokenValid() : false;

      if (isTokenValid && user) {
        state.isAuthenticated = true;
        state.user = user;
        state.accessToken = accessToken;
      } else {
        state.isAuthenticated = false;
        state.user = null;
        state.accessToken = null;
        tokenStorage.clearAuthData();
      }
    }
  }
});

export const {
  setCredentials,
  clearCredentials,
  setAuthError,
  restoreAuthStateFromLocalStorage 
} = authSlice.actions;

export default authSlice.reducer;