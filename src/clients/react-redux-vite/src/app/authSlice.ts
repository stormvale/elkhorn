import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { tokenStorage } from '../utils/tokenStorage';
//import { User } from '../types';
import { UserSchoolDto } from '../features/users/api/apiSlice-generated';
import { User } from '../types';

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
      currentSchool: null, // Will be set separately
      availableSchools: [],
    };
  } else {
    tokenStorage.clearAuthData();
    return {
      isAuthenticated: false,
      user: null,
      accessToken: null,
      error: null,
      currentSchool: null,
      availableSchools: [],
    };
  }
};

const initialState: AuthState = initializeAuthState();

/*
 * This slice manages authentication state, including the access token obtained via MSAL
 * and user information. It provides actions to set and clear credentials.
 */

interface AuthState {
  // Authentication
  isAuthenticated: boolean;
  user: User | null;
  accessToken: string | null;
  error: string | null;
  
  // School Context
  currentSchool: UserSchoolDto | null;
  availableSchools: UserSchoolDto[];
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
      state.currentSchool = null;
      state.availableSchools = [];
      tokenStorage.clearAuthData();
    },

    setSchoolContext: (state, action: PayloadAction<{ schools: UserSchoolDto[], currentSchoolId?: string }>) => {
      const { schools, currentSchoolId } = action.payload;
      state.availableSchools = schools;
      
      // Set current school if provided
      if (currentSchoolId) {
        const school = schools.find(s => s.id === currentSchoolId);
        state.currentSchool = school || null;
      } else {
        // otherwise try to restore from session storage
        const storedSchoolId = sessionStorage.getItem('schoolId') ;
        if (storedSchoolId) {
          const school = schools.find(s => s.id === storedSchoolId);
          state.currentSchool = school || null;
        }
        else{
          // otherwise just use the first school
          state.currentSchool = schools[0] || null;
        }
      }
    },

    setCurrentSchool: (state, action: PayloadAction<UserSchoolDto>) => {
      const school = action.payload;
      state.currentSchool = school;
      sessionStorage.setItem('schoolId', school.id);
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
        state.currentSchool = null;
        state.availableSchools = [];
        tokenStorage.clearAuthData();
      }
    }
  }
});

export const {
  setCredentials,
  clearCredentials,
  setAuthError,
  restoreAuthStateFromLocalStorage,
  setSchoolContext,
  setCurrentSchool 
} = authSlice.actions;

export default authSlice.reducer;